using Docnet.Core.Models;
using Xunit;

namespace NugetUsageAnyCpu
{
    [Collection("Example collection")]
    public class GenericExamples
    {
        private const string Path = "Assets/wikipedia_0.pdf";

        private readonly ExampleFixture _fixture;

        public GenericExamples(ExampleFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetPageCount_WhenCalled_ShouldSucceed()
        {
            using var docReader = _fixture.DocNet.GetDocReader(
                Path,
                new PageDimensions(1080, 1920));

            Assert.Equal(20, docReader.GetPageCount());
        }
    }
}
