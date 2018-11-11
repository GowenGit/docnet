using System;
using System.Linq;
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

        [Theory]
        [InlineData("Docs/simple_2.pdf", 1, "2")]
        [InlineData("Docs/simple_2.pdf", 3, "4 CONTENTS")]
        [InlineData("Docs/simple_4.pdf", 0, "")]
        [InlineData("Docs/simple_5.pdf", 0, "test.md 11/11/2018\r\n1 / 1\r\nTest document")]
        public void GetText_WhenCalled_ShouldReturnValidText(string filePath, int pageIndex, string expectedText)
        {
            ExecuteForDocument(filePath, null, 10, 10, pageIndex, pageReader =>
            {
                var text = pageReader.GetText();

                Assert.Equal(expectedText, text);
            });
        }

        [Theory]
        [InlineData("Docs/simple_3.pdf", null, 1, "Simple PDF File 2")]
        [InlineData("Docs/simple_3.pdf", null, 1, "Boring. More,")]
        [InlineData("Docs/simple_3.pdf", null, 1, "The end, and just as well.")]
        [InlineData("Docs/simple_0.pdf", null, 4, "ASCIIHexDecode")]
        [InlineData("Docs/protected_0.pdf", "password", 0, "The Secret (2016 film)")]
        public void GetText_WhenCalled_ShouldContainValidText(string filePath, string password, int pageIndex,
            string expectedText)
        {
            ExecuteForDocument(filePath, password, 10, 10, pageIndex, pageReader =>
            {
                var text = pageReader.GetText();

                Assert.Contains(expectedText, text);
            });
        }

        [Theory]
        [InlineData("Docs/simple_2.pdf", null, 1, 1)]
        [InlineData("Docs/simple_2.pdf", null, 3, 10)]
        [InlineData("Docs/simple_5.pdf", null, 0, 40)]
        [InlineData("Docs/protected_0.pdf", "password", 0, 2009)]
        public void GetCharacters_WhenCalled_ShouldReturnCharacters(string filePath, string password, int pageIndex, int charCount)
        {
            ExecuteForDocument(filePath, password, 10, 10, pageIndex, pageReader =>
            {
                var characters = pageReader.GetCharacters().ToArray();

                Assert.Equal(charCount, characters.Length);
            });
        }

        [Theory]
        [InlineData("Docs/simple_3.pdf", null, 1)]
        [InlineData("Docs/simple_0.pdf", null, 18)]
        [InlineData("Docs/protected_0.pdf", "password", 0)]
        public void GetImage_WhenCalled_ShouldReturnNonZeroRawByteArray(string filePath, string password, int pageIndex)
        {
            ExecuteForDocument(filePath, password, 10, 10, pageIndex, pageReader =>
            {
                var bytes = pageReader.GetImage().ToArray();

                Assert.True(bytes.Length > 0);
                Assert.True(bytes.Count(x => x != 0) > 0);
            });
        }
    }
}
