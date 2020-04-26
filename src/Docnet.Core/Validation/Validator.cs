using System;

namespace Docnet.Core.Validation
{
    internal static class Validator
    {
        public static void CheckFilePathNotNull(string filePath, string name)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(name, "file path can't be null");
            }
        }

        public static void CheckBytesNullOrZero(byte[] bytes, string name)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException(name, "bytes can't be null or empty");
            }
        }

        public static void CheckNotLessOrEqualToZero(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException("value can't be less or equal to zero", name);
            }
        }

        public static void CheckNotLessOrEqualToZero(double value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException("value can't be less or equal to zero", name);
            }
        }

        public static void CheckNotLessThanZero(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentException("value can't be less than zero", name);
            }
        }

        public static void CheckNotGreaterThan(int valueOne, int valueTwo, string nameOne, string nameTwo)
        {
            if (valueOne > valueTwo)
            {
                throw new ArgumentException($"{nameOne} can't be more than {nameTwo}");
            }
        }

        public static void CheckOrder(int coordOne, int coordTwo, string nameOne, string nameTwo)
        {
            if (coordOne > coordTwo)
            {
                throw new ArgumentException($"{nameOne} coordinate can't be more than {nameTwo} coordinate");
            }
        }

        public static void ValidatePageIndices(int pageFromIndex, int pageToIndex, string nameOne, string nameTwo)
        {
            CheckNotLessThanZero(pageFromIndex, nameof(pageFromIndex));
            CheckNotLessThanZero(pageToIndex, nameof(pageToIndex));

            CheckNotGreaterThan(pageFromIndex, pageToIndex, nameof(pageFromIndex), nameof(pageToIndex));
        }

        public static void ValidatePageNumbers(string pageRange, string argName)
        {
            if (string.IsNullOrEmpty(pageRange))
            {
                throw new ArgumentException("Page range can't be null or empty", argName);
            }

            var subRanges = pageRange.Split(new[] { ',' }, StringSplitOptions.None);

            foreach (var subRange in subRanges)
            {
                if (subRange.Length == 0)
                {
                    throw new ArgumentException("Sub-range can't be empty", argName);
                }

                var bounds = subRange.Split(new[] { '-' }, StringSplitOptions.None);

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

        private static void ValidateNumber(string numberStr, string argName, string subRange = null)
        {
            if (!int.TryParse(numberStr, out var number))
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
