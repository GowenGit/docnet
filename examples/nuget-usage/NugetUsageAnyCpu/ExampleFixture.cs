using Docnet.Core;
using System;
using Xunit;

namespace NugetUsageAnyCpu
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