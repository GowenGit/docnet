using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Docnet.Core.Bindings;
using Docnet.Core.Converters;
using Docnet.Core.Exceptions;
using Docnet.Core.Models;

namespace Docnet.Core.Readers
{
    internal sealed class PageReader : IPageReader
    {
        private readonly FpdfDocumentT document;
        private readonly FpdfPageT _page;
        private readonly FpdfTextpageT _textPage;

        private readonly double _scaling;

        /// <inheritdoc />
        public int PageIndex { get; }

        public PageReader(DocumentWrapper docWrapper, int pageIndex, PageDimensions pageDimensions)
        {
            document = docWrapper.Instance;
            PageIndex = pageIndex;

            lock (DocLib.Lock)
            {
                _page = fpdf_view.FPDF_LoadPage(docWrapper.Instance, pageIndex);

                if (_page == null)
                {
                    throw new DocnetException($"failed to open page for page index {pageIndex}");
                }

                _textPage = fpdf_text.FPDFTextLoadPage(_page);

                if (_textPage == null)
                {
                    throw new DocnetException($"failed to open page text for page index {pageIndex}");
                }

                _scaling = pageDimensions.GetScalingFactor(_page);
            }
        }

        /// <inheritdoc />
        public int GetPageWidth()
        {
            lock (DocLib.Lock)
            {
                return (int)(fpdf_view.FPDF_GetPageWidth(_page) * _scaling);
            }
        }

        /// <inheritdoc />
        public int GetPageHeight()
        {
            lock (DocLib.Lock)
            {
                return (int)(fpdf_view.FPDF_GetPageHeight(_page) * _scaling);
            }
        }

        public int GetPageRotation()
        {
            lock (DocLib.Lock)
            {
                return fpdf_edit.FPDFPageGetRotation(_page);
            }
        }

        public List<(uint, uint)> GetDimensionsOfEmbeddedImages()
        {
            var imageDimensionList = new List<(uint, uint)>();

            var objCount = fpdf_edit.FPDFPageCountObjects(_page);
            for (int i = 0; i < objCount; i++)
            {
                var pageObj = fpdf_edit.FPDFPageGetObject(_page, i);
                var objectType = fpdf_edit.FPDFPageObjGetType(pageObj);

                if (objectType == PdfPageObjectType.FPDF_PAGEOBJ_IMAGE)
                {
                    FPDF_IMAGEOBJ_METADATA imageMetaData = new FPDF_IMAGEOBJ_METADATA();
                    var hasMetaData = fpdf_edit.FPDFImageObjGetImageMetadata(pageObj, _page, imageMetaData);
                    if (hasMetaData)
                    {
                        var size = (imageMetaData.Width, imageMetaData.Height);
                        imageDimensionList.Add(size);
                    }

                    imageMetaData.Dispose();
                }
            }

            return imageDimensionList;
        }

        /// <inheritdoc />
        public string GetText()
        {
            ushort[] buffer;

            int charactersWritten;

            lock (DocLib.Lock)
            {
                var charCount = fpdf_text.FPDFTextCountChars(_textPage);

                buffer = new ushort[charCount + 1];

                charactersWritten = fpdf_text.FPDFTextGetText(_textPage, 0, charCount, ref buffer[0]);
            }

            if (charactersWritten == 0)
            {
                return string.Empty;
            }

            string result;

            unsafe
            {
                fixed (ushort* dataPtr = &buffer[0])
                {
                    result = new string((char*)dataPtr, 0, charactersWritten - 1);
                }
            }

            return result;
        }

        public IEnumerable<(string uri, BoundBox bbox)> GetUriAnnotations()
        {
            lock (DocLib.Lock)
            {
                var width = GetPageWidth();
                var height = GetPageHeight();
                var annotCount = fpdf_annot.FPDFPageGetAnnotCount(_page);

                for (var i = 0; i < annotCount; i++)
                {
                    var annot = fpdf_annot.FPDFPageGetAnnot(_page, i);
                    var subtype = (FpdfAnnotationSubtype)fpdf_annot.FPDFAnnotGetSubtype(annot);
                    if (subtype != FpdfAnnotationSubtype.FPDF_ANNOT_LINK)
                    {
                        continue;
                    }

                    var link = fpdf_annot.FPDFAnnotGetLink(annot);
                    var action = fpdf_doc.FPDFLinkGetAction(link);
                    var actionType = (PdfActionType)fpdf_doc.FPDFActionGetType(action);
                    if (actionType != PdfActionType.URI)
                    {
                        continue;
                    }

                    // read uri string
                    var uriBuffer = new byte[128];
                    var uriLength = fpdf_doc.FPDFActionGetURIPath(document, action, ref uriBuffer[0], uriBuffer.Length);
                    if (uriBuffer.Length < (int)uriLength)
                    {
                        uriBuffer = new byte[uriLength + 10];
                        uriLength = fpdf_doc.FPDFActionGetURIPath(document, action, ref uriBuffer[0], uriBuffer.Length);
                    }

                    byte[] asBytes = new byte[uriLength];
                    Buffer.BlockCopy(uriBuffer, 0, asBytes, 0, asBytes.Length);
                    var uriString = Encoding.ASCII.GetString(asBytes);
                    uriString = uriString.Replace("\0", string.Empty);

                    // read annot bounding box
                    BoundBox annotBoundBox = new BoundBox(0, 0, 0, 0);

                    using (FS_RECTF_ rec = new FS_RECTF_())
                    {
                        var success2 = fpdf_annot.FPDFAnnotGetRect(annot, rec) == 1;
                        if (success2)
                        {
                            var (left, top) = GetAdjustedCoords(width, height, rec.Left, rec.Top);
                            var (right, bottom) = GetAdjustedCoords(width, height, rec.Right, rec.Bottom);

                            var adjLeft = Math.Min(left, right);
                            var adjRight = Math.Max(left, right);
                            var adjTop = Math.Min(top, bottom);
                            var adjBottom = Math.Max(top, bottom);

                            annotBoundBox = new BoundBox(adjLeft, adjTop, adjRight, adjBottom);

                            rec.Dispose();
                        }
                    }

                    fpdf_annot.FPDFPageCloseAnnot(annot);

                    yield return (uriString, annotBoundBox);
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<Character> GetCharacters()
        {
            lock (DocLib.Lock)
            {
                var width = GetPageWidth();
                var height = GetPageHeight();

                var charCount = fpdf_text.FPDFTextCountChars(_textPage);

                for (var i = 0; i < charCount; i++)
                {
                    var charCode = (char)fpdf_text.FPDFTextGetUnicode(_textPage, i);
                    var angle = fpdf_text.FPDFTextGetCharAngle(_textPage, i);
                    var renderMode = (TextRenderMode)fpdf_text.FPDFTextGetTextRenderMode(_textPage, i);
                    var pageRotation = GetPageRotation();

                    var fontSize = fpdf_text.FPDFTextGetFontSize(_textPage, i);
                    var adjustedFontSize = fontSize != 1 || fontSize != 1.2857141494750977 ?
                        fontSize * _scaling : fontSize;

                    // get font type + some info about it.
                    string fontInfo = string.Empty;
                    var fontBuffer = new byte[128];
                    int fontFlags = 0;
                    var fontTextLen = fpdf_text.FPDFTextGetFontInfo(_textPage, i, ref fontBuffer[0], (ulong)fontBuffer.Length, ref fontFlags);
                    if (fontTextLen > 0)
                    {
                        if (fontBuffer.Length < fontTextLen)
                        {
                            fontBuffer = new byte[fontTextLen];
                            fontTextLen = fpdf_text.FPDFTextGetFontInfo(_textPage, i, ref fontBuffer[0], (ulong)fontBuffer.Length, ref fontFlags);
                        }

                        byte[] asBytes = new byte[fontTextLen];
                        Buffer.BlockCopy(fontBuffer, 0, asBytes, 0, asBytes.Length);
                        var str = Encoding.UTF8.GetString(asBytes);
                        fontInfo = str.Length > 0 && str.Substring(str.Length - 1) == "\0" ? str.Substring(0, str.Length - 1) : str;
                    }

                    uint r = 0, g = 0, b = 0, a = 0;
                    fpdf_text.FPDFTextGetStrokeColor(_textPage, i, ref r, ref g, ref b, ref a);
                    var strokeColor = new List<uint>(4) { r, g, b, a };

                    switch (pageRotation)
                    {
                        case 0:
                            break;
                        case 1:
                            angle += (float)(Math.PI / 2);
                            break;
                        case 2:
                            angle += (float)Math.PI;
                            break;
                        case 3:
                            angle += (float)(Math.PI * 3 / 2);
                            break;
                    }

                    double originX = 0;
                    double originY = 0;
                    var origin = fpdf_text.FPDFTextGetCharOrigin(_textPage, i, ref originX, ref originY);
                    var (adjustedOriginX, adjustedOriginY) = GetAdjustedCoords(width, height, originX, originY);

                    double left = 0;
                    double top = 0;
                    double right = 0;
                    double bottom = 0;

                    var success = fpdf_text.FPDFTextGetCharBox(_textPage, i, ref left, ref right, ref bottom, ref top) == 1;

                    if (!success)
                    {
                        continue;
                    }

                    var (adjustedLeft, adjustedTop) = GetAdjustedCoords(width, height, left, top);
                    var (adjustRight, adjustBottom) = GetAdjustedCoords(width, height, right, bottom);

                    var fixAdjustedLeft = Math.Min(adjustedLeft, adjustRight);
                    var fixAdjustRight = Math.Max(adjustedLeft, adjustRight);
                    var fixAdjustedTop = Math.Min(adjustedTop, adjustBottom);
                    var fixAdjustBottom = Math.Max(adjustedTop, adjustBottom);

                    var box = new BoundBox(fixAdjustedLeft, fixAdjustedTop, fixAdjustRight, fixAdjustBottom);

                    BoundBox looseBox;

                    using (FS_RECTF_ rec = new FS_RECTF_())
                    {
                        var success2 = fpdf_text.FPDFTextGetLooseCharBox(_textPage, i, rec) == 1;
                        if (!success2)
                        {
                            continue;
                        }

                        var (adjLooseLeft, adjLooseTop) = GetAdjustedCoords(width, height, rec.Left, rec.Top);
                        var (adjLooseRight, adjLooseBottom) = GetAdjustedCoords(width, height, rec.Right, rec.Bottom);

                        adjLooseLeft = Math.Min(adjLooseLeft, adjLooseRight);
                        adjLooseRight = Math.Max(adjLooseLeft, adjLooseRight);
                        adjLooseTop = Math.Min(adjLooseTop, adjLooseBottom);
                        adjLooseBottom = Math.Max(adjLooseTop, adjLooseBottom);

                        looseBox = new BoundBox(adjLooseLeft, adjLooseTop, adjLooseRight, adjLooseBottom);

                        rec.Dispose();
                    }

                    yield return new Character(charCode, fontInfo, adjustedFontSize, angle, renderMode, box, looseBox, adjustedOriginX, adjustedOriginY, strokeColor);
                }
            }
        }

        private (int x, int y) GetAdjustedCoords(int width, int height, double pageX, double pageY)
        {
            var x = 0;
            var y = 0;

            fpdf_view.FPDF_PageToDevice(
                _page,
                0,
                0,
                width,
                height,
                0,
                pageX,
                pageY,
                ref x,
                ref y);

            x = AdjustToRange(x, width);
            y = AdjustToRange(y, height);

            return (x, y);
        }

        private static int AdjustToRange(int coord, int range)
        {
            if (coord < 0)
            {
                coord = 0;
            }

            if (coord >= range)
            {
                coord = range - 1;
            }

            return coord;
        }

        /// <inheritdoc />
        public byte[] GetImage() => GetImage(0);

        /// <inheritdoc />
        public byte[] GetImage(RenderFlags flags)
        {
            lock (DocLib.Lock)
            {
                var width = GetPageWidth();
                var height = GetPageHeight();

                var bitmap = fpdf_view.FPDFBitmapCreate(width, height, 1);

                if (bitmap == null)
                {
                    throw new DocnetException("failed to create a bitmap object");
                }

                var stride = fpdf_view.FPDFBitmapGetStride(bitmap);

                var result = new byte[stride * height];

                try
                {
                    // |          | a b 0 |
                    // | matrix = | c d 0 |
                    // |          | e f 1 |
                    using (var matrix = new FS_MATRIX_())
                    using (var clipping = new FS_RECTF_())
                    {
                        matrix.A = (float)_scaling;
                        matrix.B = 0;
                        matrix.C = 0;
                        matrix.D = (float)_scaling;
                        matrix.E = 0;
                        matrix.F = 0;

                        clipping.Left = 0;
                        clipping.Right = width;
                        clipping.Bottom = 0;
                        clipping.Top = height;

                        fpdf_view.FPDF_RenderPageBitmapWithMatrix(bitmap, _page, matrix, clipping, (int)flags);

                        var buffer = fpdf_view.FPDFBitmapGetBuffer(bitmap);

                        Marshal.Copy(buffer, result, 0, result.Length);
                    }
                }
                catch (Exception ex)
                {
                    throw new DocnetException("error rendering page", ex);
                }
                finally
                {
                    fpdf_view.FPDFBitmapDestroy(bitmap);
                }

                return result;
            }
        }

        /// <inheritdoc />
        public byte[] GetImage(IImageBytesConverter converter) => GetImage(converter, 0);

        /// <inheritdoc />
        public byte[] GetImage(IImageBytesConverter converter, RenderFlags flags)
        {
            var bytes = GetImage(flags);

            return converter.Convert(bytes);
        }

        public void Dispose()
        {
            lock (DocLib.Lock)
            {
                if (_textPage != null)
                {
                    fpdf_text.FPDFTextClosePage(_textPage);
                }

                if (_page == null)
                {
                    return;
                }

                fpdf_view.FPDF_ClosePage(_page);
            }
        }
    }
}