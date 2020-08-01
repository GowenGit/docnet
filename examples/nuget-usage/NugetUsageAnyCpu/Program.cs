using System;
using Docnet.Core;
using Xunit;

namespace NugetUsageAnyCpu
{
    [Collection("Example collection")]
    public class PdfToImageExamples
    {
        private readonly ExampleFixture _fixture;

        public PdfToImageExamples(ExampleFixture fixture)
        {
            _fixture = fixture;
        }
    }

    [Collection("Example collection")]
    public class ImageToPdfExamples
    {
        private readonly ExampleFixture _fixture;

        public ImageToPdfExamples(ExampleFixture fixture)
        {
            _fixture = fixture;
        }
    }

    [Collection("Example collection")]
    public class GenericExamples
    {
        private readonly ExampleFixture _fixture;

        public GenericExamples(ExampleFixture fixture)
        {
            _fixture = fixture;
        }
    }

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
