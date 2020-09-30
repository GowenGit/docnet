using System.IO;
using Docnet.Core.Models;
using Docnet.Core.Readers;

namespace Docnet.Tests.Integration.Utils
{
    internal static class SutHelpers
    {
        public static IDocReader GetDocReader(this LibFixture fixture, Input type, string filePath, string password, int dimOne, int dimTwo)
        {
            if (type == Input.FromFile)
            {
                return fixture.Lib.GetDocReader(filePath, password, new PageDimensions(dimOne, dimTwo));
            }

            var bytes = File.ReadAllBytes(filePath);

            return fixture.Lib.GetDocReader(bytes, password, new PageDimensions(dimOne, dimTwo));
        }

        public static IDocReader GetDocReader(this LibFixture fixture, Input type, string filePath, string password, double scaling)
        {
            if (type == Input.FromFile)
            {
                return fixture.Lib.GetDocReader(filePath, password, new PageDimensions(scaling));
            }

            var bytes = File.ReadAllBytes(filePath);

            return fixture.Lib.GetDocReader(bytes, password, new PageDimensions(scaling));
        }

        public static byte[] Split(this LibFixture fixture, Input type, string filePath, int fromIndex, int toIndex)
        {
            if (type == Input.FromFile)
            {
                return fixture.Lib.Split(filePath, fromIndex, toIndex);
            }

            var bytes = File.ReadAllBytes(filePath);

            return fixture.Lib.Split(bytes, fromIndex, toIndex);
        }

        public static byte[] Split(this LibFixture fixture, Input type, string filePath, string pageRange)
        {
            if (type == Input.FromFile)
            {
                return fixture.Lib.Split(filePath, pageRange);
            }

            var bytes = File.ReadAllBytes(filePath);

            return fixture.Lib.Split(bytes, pageRange);
        }
    }
}