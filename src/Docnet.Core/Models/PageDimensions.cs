using System;
using Docnet.Core.Bindings;
using Docnet.Core.Validation;

namespace Docnet.Core.Models
{
    /// <summary>
    /// Page dimension options. Configures how
    /// rendered page images should scale and be
    /// transformed in a pixel space.
    /// </summary>
    public struct PageDimensions
    {
        public int DimOne { get; }

        public int DimTwo { get; }

        private double? _scalingFactor;

        /// <summary>
        /// Get page dimension options for this particular document.
        /// dimOne x dimTwo represents a viewport to which
        /// the document gets scaled to fit without modifying
        /// it's aspect ratio.
        /// </summary>
        /// <param name="dimOne">Smaller dimension.</param>
        /// <param name="dimTwo">Larger dimension.</param>
        public PageDimensions(int dimOne, int dimTwo)
        {
            Validator.CheckNotLessOrEqualToZero(dimOne, nameof(dimOne));
            Validator.CheckNotLessOrEqualToZero(dimTwo, nameof(dimTwo));

            Validator.CheckNotGreaterThan(dimOne, dimTwo, nameof(dimOne), nameof(dimTwo));

            DimOne = dimOne;
            DimTwo = dimTwo;
            _scalingFactor = null;
        }

        /// <summary>
        /// Get page dimension options for this particular document.
        /// scalingFactor represents a value to which physical page
        /// dimensions should scale in a pixel space.
        /// </summary>
        /// <param name="scalingFactor">Page scaling factor in pixels-per-point. Convert PPI to scaling factor as PPI/72.</param>
        public PageDimensions(double scalingFactor)
        {
            Validator.CheckNotLessOrEqualToZero(scalingFactor, nameof(scalingFactor));

            DimOne = 0;
            DimTwo = 0;
            _scalingFactor = scalingFactor;
        }

        /// <summary>
        /// Gets rescaling factor for native width x height of the page
        /// so it maximizes the dimOne x dimTwo rectangle
        ///
        /// Note: make sure that the semaphore is locked before access.
        /// </summary>
        /// <param name="page">Page object.</param>
        /// <returns>Scaling factor.</returns>
        internal double GetScalingFactor(FpdfPageT page)
        {
            if (_scalingFactor != null)
            {
                return _scalingFactor.Value;
            }

            var width = fpdf_view.FPDF_GetPageWidth(page);
            var height = fpdf_view.FPDF_GetPageHeight(page);

            var scaleOne = DimOne / Math.Min(width, height);
            var scalingTwo = DimTwo / Math.Max(width, height);

            return Math.Min(scaleOne, scalingTwo);
        }
    }
}