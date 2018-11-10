using System;

namespace Docnet.Core.Models
{
    public struct Version
    {
        public int Number { get; }

        public Version(int number)
        {
            if (number < 0)
            {
                throw new ArgumentException("version can't be less than 0", nameof(number));
            }

            if (number > 99)
            {
                throw new ArgumentException("version can't be greater than 99", nameof(number));
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
            if (!(obj is Version))
            {
                return false;
            }

            var version = (Version)obj;
            return Number == version.Number;
        }

        public override int GetHashCode()
        {
            return 91 + Number.GetHashCode();
        }
    }
}
