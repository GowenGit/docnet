using System;
using System.Linq;

namespace Docnet.Core.Models
{
    public struct PdfVersion
    {
        private static readonly int[] SupportedVersions =
        {
            10, 11, 12, 13, 14, 15, 16, 17, 20
        };

        public int Number { get; }

        public PdfVersion(int number)
        {
            if (!SupportedVersions.Contains(number))
            {
                throw new ArgumentException($"version is not supported. should be one of {string.Join(", ", SupportedVersions)}");
            }

            Number = number;
        }

        public override string ToString()
        {
            var major = Number / 10;
            var minor = Number % 10;

            return $"{major}.{minor}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PdfVersion))
            {
                return false;
            }

            var version = (PdfVersion)obj;
            return Number == version.Number;
        }

        public override int GetHashCode()
        {
            return Number;
        }
    }
}
