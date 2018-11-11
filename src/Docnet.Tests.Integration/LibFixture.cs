using System;
using Docnet.Core;
using Xunit;

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

    [CollectionDefinition("Lib collection")]
    public class LibCollection : ICollectionFixture<LibFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}