using System;

namespace Docnet.Core.Validation
{
    public class PageRangeValidator : IPageRangeValidator
    {
        public void ValidatePageIndices(int pageFromIndex, int pageToIndex, string nameOne, string nameTwo)
        {
            Validator.CheckNotLessThanZero(pageFromIndex, nameof(pageFromIndex));
            Validator.CheckNotLessThanZero(pageToIndex, nameof(pageToIndex));

            Validator.CheckNotGreaterThan(pageFromIndex, pageToIndex, nameof(pageFromIndex), nameof(pageToIndex));
        }

        public void ValidatePageNumbers(string pageRange, string argName)
        {
            if (string.IsNullOrEmpty(pageRange))
            {
                throw new ArgumentException("Page range can't be null or empty", argName);
            }

            string[] subRanges = pageRange.Split(new[] {','}, StringSplitOptions.None);

            foreach (string subRange in subRanges)
            {
                if (subRange.Length == 0)
                {
                    throw new ArgumentException("Sub-range can't be empty", argName);
                }

                string[] bounds = subRange.Split(new[] { '-' }, StringSplitOptions.None);

                if (bounds.Length == 1)
                {
                    ValidateNumber(subRange, argName);
                    continue;
                }

                if (bounds.Length > 2)
                {
                    throw new ArgumentException($"Sub-range must contain only one lower and upper bounds (sub-range [{subRange}])", argName);
                }

                ValidateNumber(bounds[0], argName, subRange);
                ValidateNumber(bounds[1], argName, subRange);
            }
        }

        private void ValidateNumber(string numberStr, string argName, string subRange = null)
        {
            if (!int.TryParse(numberStr, out int number))
            {
                throw new ArgumentException($"[{numberStr}] is not a number {(subRange != null ? $"(sub-range [{subRange}])" : string.Empty)}", argName);
            }

            if (number <= 0)
            {
                throw new ArgumentException($"Page number must be greater than 0 {(subRange != null ? $"(sub-range [{subRange}])" : string.Empty)}", argName);
            }
        }
    }
}