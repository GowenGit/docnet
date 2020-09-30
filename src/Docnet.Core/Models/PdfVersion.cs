using System;
using System.Collections.Generic;

namespace Docnet.Core.Models
{
    public struct PdfVersion : IEquatable<PdfVersion>
    {
        private static readonly HashSet<int> SupportedVersions = new HashSet<int>
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

        public static bool operator ==(PdfVersion obj1, PdfVersion obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(PdfVersion obj1, PdfVersion obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(PdfVersion other)
        {
            return Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PdfVersion))
            {
                return false;
            }

            var version = (PdfVersion)obj;

            return Equals(version);
        }

        public override int GetHashCode()
        {
            return Number;
        }
    }
}
