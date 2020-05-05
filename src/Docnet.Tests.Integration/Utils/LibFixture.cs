using System;
using Docnet.Core;
using Xunit;

#pragma warning disable CA1711

namespace Docnet.Tests.Integration.Utils
{
    public sealed class LibFixture : IDisposable
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