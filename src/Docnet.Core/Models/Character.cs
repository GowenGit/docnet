using System;

namespace Docnet.Core.Models
{
    public struct Character : IEquatable<Character>
    {
        private const double Tolerance = 0.001;

        public char Char { get; }

        public BoundBox Box { get; }

        public float Angle { get; }

        public double FontSize { get; }

        public Character(char character, BoundBox box, float angle, double fontSize)
        {
            Char = character;
            Box = box;
            Angle = angle;
            FontSize = fontSize;
        }

        public static bool operator ==(Character obj1, Character obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Character obj1, Character obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Character other)
        {
            return Char == other.Char
                   && Box.Equals(other.Box)
                   && Math.Abs(Angle - other.Angle) < Tolerance
                   && Math.Abs(FontSize - other.FontSize) < Tolerance;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Character))
            {
                return false;
            }

            var character = (Character)obj;

            return Equals(character);
        }

        public override int GetHashCode()
        {
            var hashCode = 13;
            hashCode = hashCode * 7 + Char.GetHashCode();
            hashCode = hashCode * 7 + Box.GetHashCode();
            return hashCode;
        }
    }
}