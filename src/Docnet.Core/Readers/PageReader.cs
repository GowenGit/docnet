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

                    var box = new BoundBox(adjustedLeft, adjustedTop, adjustRight, adjustBottom);

                    yield return new Character(charCode, box);
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
        public byte[] GetImage()
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

                        fpdf_view.FPDF_RenderPageBitmapWithMatrix(bitmap, _page, matrix, clipping, 0);

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
        public byte[] GetImage(IImageBytesConverter converter)
        {
            var bytes = GetImage();

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