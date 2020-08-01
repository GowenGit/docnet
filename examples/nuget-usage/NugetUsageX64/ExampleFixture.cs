using System;
using Docnet.Core;
using Xunit;

namespace NugetUsageX64
{
    public class ExampleFixture : IDisposable
    {
        public IDocLib DocNet { get; }

        public ExampleFixture()
        {
            DocNet = DocLib.Instance;
        }

        public void Dispose()
        {
            DocNet.Dispose();
        }
    }

    [CollectionDefinition("Example collection")]
    public class ExampleCollection : ICollectionFixture<ExampleFixture> { }
}