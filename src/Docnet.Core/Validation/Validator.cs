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

        public static void CheckNotLessOrEqualToZero(int value, string name)
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
    }
}
