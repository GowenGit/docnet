using System;
using Docnet.Core.Bindings;
using Docnet.Core.Exceptions;

namespace Docnet.Core.Readers
{
    internal sealed class PageReader : IPageReader
    {
        private readonly DocumentWrapper _docWrapper;

        private readonly FpdfPageT _page;

        private readonly double _scaling;

        public int PageIndex { get; }

        public PageReader(DocumentWrapper docWrapper, int pageIndex, int dimOne, int dimTwo)
        {
            _docWrapper = docWrapper;
            PageIndex = pageIndex;

            _page = fpdf_view.FPDF_LoadPage(docWrapper.Instance, pageIndex);

            if (_page == null)
            {
                throw new DocnetException($"failed to open page for page index {pageIndex}");
            }

            _scaling = GetScalingFactor(_page, dimOne, dimTwo);
        }

        public int GetPageWidth()
        {
            return (int)(fpdf_view.FPDF_GetPageWidth(_page) * _scaling);
        }

        public int GetPageHeight()
        {
            return (int)(fpdf_view.FPDF_GetPageHeight(_page) * _scaling);
        }

        private static double GetScalingFactor(FpdfPageT page, int dimOne, int dimTwo)
        {
            var width = fpdf_view.FPDF_GetPageWidth(page);
            var height = fpdf_view.FPDF_GetPageHeight(page);

            var scaleOne = dimOne / Math.Max(width, height);
            var scalingTwo = dimTwo / Math.Max(width, height);

            return Math.Min(scaleOne, scalingTwo);
        }

        public void Dispose()
        {
            if (_page == null)
            {
                return;
            }

            fpdf_view.FPDF_ClosePage(_page);
        }
    }
}