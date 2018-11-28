using Docnet.Core.Validation;

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
            Validator.CheckNotLessThanZero(left, nameof(left));
            Validator.CheckNotLessThanZero(top, nameof(top));
            Validator.CheckNotLessThanZero(right, nameof(right));
            Validator.CheckNotLessThanZero(bottom, nameof(bottom));

            Validator.CheckOrder(left, right, nameof(left), nameof(right));
            Validator.CheckOrder(top, bottom, nameof(top), nameof(bottom));

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
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