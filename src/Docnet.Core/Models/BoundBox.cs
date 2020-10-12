using System;
using Docnet.Core.Validation;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
namespace Docnet.Core.Models
{
    public struct BoundBox : IEquatable<BoundBox>
    {
        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public BoundBox(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static bool operator ==(BoundBox obj1, BoundBox obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(BoundBox obj1, BoundBox obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(BoundBox other)
        {
            return Left == other.Left &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoundBox))
            {
                return false;
            }

            var box = (BoundBox)obj;

            return Equals(box);
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