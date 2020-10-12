using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Docnet.Core.Bindings;
using Docnet.Core.Converters;
using Docnet.Core.Exceptions;
using Docnet.Core.Models;

namespace Docnet.Core.Readers
{
    internal sealed class PageReader : IPageReader
    {
        private readonly FpdfPageT _page;
        private readonly FpdfTextpageT _text;

        private readonly double _scaling;

        /// <inheritdoc />
        public int PageIndex { get; }

        public PageReader(DocumentWrapper docWrapper, int pageIndex, PageDimensions pageDimensions)
        {
            PageIndex = pageIndex;

            lock (DocLib.Lock)
            {
                _page = fpdf_view.FPDF_LoadPage(docWrapper.Instance, pageIndex);

                if (_page == null)
                {
                    throw new DocnetException($"failed to open page for page index {pageIndex}");
                }

                _text = fpdf_text.FPDFTextLoadPage(_page);

                if (_text == null)
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

        /// <inheritdoc />
        public string GetText()
        {
            ushort[] buffer;

            int charactersWritten;

            lock (DocLib.Lock)
            {
                var charCount = fpdf_text.FPDFTextCountChars(_text);

                buffer = new ushort[charCount + 1];

                charactersWritten = fpdf_text.FPDFTextGetText(_text, 0, charCount, ref buffer[0]);
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

        /// <inheritdoc />
        public IEnumerable<Character> GetCharacters()
        {
            lock (DocLib.Lock)
            {
                var width = GetPageWidth();
                var height = GetPageHeight();

                var charCount = fpdf_text.FPDFTextCountChars(_text);

                for (var i = 0; i < charCount; i++)
                {
                    var charCode = (char)fpdf_text.FPDFTextGetUnicode(_text, i);
                    var fontSize = fpdf_text.FPDFTextGetFontSize(_text, i);
                    var angle = fpdf_text.FPDFTextGetCharAngle(_text, i);
                    var renderMode = (TextRenderMode)fpdf_text.FPDFTextGetTextRenderMode(_text, i);
                    var pageRotation = GetPageRotation();

                    uint r = 0, g = 0, b = 0, a = 0;
                    fpdf_text.FPDFTextGetStrokeColor(_text, i, ref r, ref g, ref b, ref a);
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
                    var origin = fpdf_text.FPDFTextGetCharOrigin(_text, i, ref originX, ref originY);

                    var (adjustedOriginX, adjustedOriginY) = GetAdjustedCoords(width, height, originX, originY);

                    double left = 0;
                    double top = 0;
                    double right = 0;
                    double bottom = 0;

                    var success = fpdf_text.FPDFTextGetCharBox(_text, i, ref left, ref right, ref bottom, ref top) == 1;

                    if (!success)
                    {
                        continue;
                    }

                    var (adjustedLeft, adjustedTop) = GetAdjustedCoords(width, height, left, top);
                    var (adjustRight, adjustBottom) = GetAdjustedCoords(width, height, right, bottom);

                    adjustedLeft = Math.Min(adjustedLeft, adjustRight);
                    adjustRight = Math.Max(adjustedLeft, adjustRight);
                    adjustedTop = Math.Min(adjustedTop, adjustBottom);
                    adjustBottom = Math.Max(adjustedTop, adjustBottom);

                    var box = new BoundBox(adjustedLeft, adjustedTop, adjustRight, adjustBottom);

                    BoundBox looseBox;

                    using (FS_RECTF_ rec = new FS_RECTF_())
                    {
                        var success2 = fpdf_text.FPDFTextGetLooseCharBox(_text, i, rec) == 1;
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

                    yield return new Character(charCode, fontSize, angle, renderMode, box, looseBox, adjustedOriginX, adjustedOriginY, strokeColor);
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
                if (_text != null)
                {
                    fpdf_text.FPDFTextClosePage(_text);
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