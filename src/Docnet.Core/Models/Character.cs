using System;
using System.Collections.Generic;

namespace Docnet.Core.Models
{
    public struct Character : IEquatable<Character>
    {
        private const double Tolerance = 0.001;

        public char Char { get; set; }

        public string FontInfo { get; set; }

        public BoundBox Box { get; set; }

        public BoundBox LooseBox { get; set; }

        public double FontSize { get; set; }

        public double Angle { get; set; }

        public double OriginX { get; set; }

        public double OriginY { get; set; }

        public List<uint> ColorARGB { get; set; }

        public TextRenderMode RenderMode { get; set; }

        public Character(char character, string fontInfo, double fontSize, double angle, TextRenderMode renderMode, BoundBox box, BoundBox looseBox, double originX, double originY, List<uint> colorARGB)
        {
            if (colorARGB.Count != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(colorARGB) + " needs to have a length of 4");
            }

            Char = character;
            FontInfo = fontInfo;
            FontSize = fontSize;
            Box = box;
            LooseBox = looseBox;
            Angle = angle;
            RenderMode = renderMode;
            OriginX = originX;
            OriginY = originY;
            Angle = angle;
            FontSize = fontSize;
            ColorARGB = colorARGB;
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