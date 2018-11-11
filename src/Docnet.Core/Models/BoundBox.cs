using System;
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Docnet.Core.Models
{
    public struct BoundBox
    {
        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public BoundBox(int left, int top, int right, int bottom)
        {
            CheckNotLessThanZero(left, nameof(left));
            CheckNotLessThanZero(top, nameof(top));
            CheckNotLessThanZero(right, nameof(right));
            CheckNotLessThanZero(bottom, nameof(bottom));

            CheckOrder(left, right, nameof(left), nameof(right));
            CheckOrder(top, bottom, nameof(top), nameof(bottom));

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        private static void CheckNotLessThanZero(int coordinate, string name)
        {
            if (coordinate < 0)
            {
                throw new ArgumentException("coordinate can't be less than 0", name);
            }
        }

        private static void CheckOrder(int coordOne, int coordTwo, string nameOne, string nameTwo)
        {
            if (coordOne > coordTwo)
            {
                throw new ArgumentException($"{nameOne} coordinate can't be more than {nameTwo} coordinate");
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoundBox))
            {
                return false;
            }

            var box = (BoundBox)obj;

            return Left == box.Left &&
                   Top == box.Top &&
                   Right == box.Right &&
                   Bottom == box.Bottom;
        }

        public override int GetHashCode()
        {
            var hashCode = 13;
            hashCode = hashCode * 7 + Left.GetHashCode();
            hashCode = hashCode * 7 + Top.GetHashCode();
            hashCode = hashCode * 7 + Right.GetHashCode();
            hashCode = hashCode * 7 + Bottom.GetHashCode();
            return hashCode;
        }
    }
}