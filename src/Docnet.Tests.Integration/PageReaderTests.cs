using System;
using Docnet.Core.Exceptions;
using Docnet.Core.Readers;
using Xunit;

namespace Docnet.Tests.Integration
{
    [Collection("Lib collection")]
    public sealed class PageReaderTests
    {
        private readonly LibFixture _fixture;

        public PageReaderTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        private void ExecuteForDocument(string filePath, string password, int dimOne, int dimTwo, int pageIndex, Action<IPageReader> action)
        {
            using (var docReader = _fixture.Lib.GetDocReader(filePath, password, dimOne, dimTwo))
            using (var pageReader = docReader.GetPageReader(pageIndex))
            {
                action(pageReader);
            }
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", null, -1)]
        [InlineData("Docs/simple_0.pdf", null, 19)]
        [InlineData("Docs/protected_0.pdf", null, -3)]
        [InlineData("Docs/protected_0.pdf", null, 10)]
        public void GetPageReader_WhenCalledWithInvalidIndex_ShouldThrow(string filePath, string password, int pageIndex)
        {
            Assert.Throws<DocnetException>(() => ExecuteForDocument(filePath, password, 10, 10, pageIndex, _ => { }));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", null, 1, 1)]
        [InlineData("Docs/simple_0.pdf", null, 10, 30)]
        [InlineData("Docs/simple_0.pdf", null, 15, 40)]
        [InlineData("Docs/simple_0.pdf", null, 1337, 1337)]
        [InlineData("Docs/simple_0.pdf", null, 1997, 1000000)]
        public void GetPageWidthOrHeight_WhenCalled_ShouldMachOneOfTheDimensions(string filePath, string password, int dimOne, int dimTwo)
        {
            ExecuteForDocument(filePath, password, dimOne, dimTwo, 0, pageReader =>
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                var min = Math.Min(width, height);
                var max = Math.Max(width, height);

                Assert.True(min == dimOne || max == dimTwo);
                Assert.True(min <= dimOne && max <= dimTwo);
            });
        }

        [Fact]
        public void GetPageWidthOrHeight_WhenCalled_ShouldKeepAspectRatio()
        {
            ExecuteForDocument("Docs/simple_0.pdf", null, 500, 10000, 3, pageReader =>
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                Assert.Equal(1.414, (double)height / width, 3);
            });
        }

        [Fact]
        public void PageIndex_WhenCalled_ShouldReturnCorrectIndex()
        {
            var random = new Random();

            var index = random.Next(19);

            ExecuteForDocument("Docs/simple_0.pdf", null, 10, 10, index, pageReader =>
            {
                Assert.Equal(index, pageReader.PageIndex);
            });
        }
    }
}
