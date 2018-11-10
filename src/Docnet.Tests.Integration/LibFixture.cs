using System;
using Docnet.Core;

namespace Docnet.Tests.Integration
{
    public class LibFixture : IDisposable
    {
        public LibFixture()
        {
            Lib = DocLib.Instance;
        }

        public void Dispose()
        {
            Lib.Dispose();
        }

        public IDocLib Lib { get; }
    }
}