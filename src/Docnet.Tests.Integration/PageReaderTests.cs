using System;
using System.Linq;
using System.Threading.Tasks;
using Docnet.Core.Converters;
using Docnet.Core.Exceptions;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using Docnet.Tests.Integration.Utils;
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

        private void ExecuteForDocument(Input type, string filePath, string password, int dimOne, int dimTwo, int pageIndex, Action<IPageReader> action)
        {
            using (var docReader = _fixture.GetDocReader(type, filePath, password, dimOne, dimTwo))
            using (var pageReader = docReader.GetPageReader(pageIndex))
            {
                action(pageReader);
            }
        }

        private void ExecuteForDocument(Input type, string filePath, string password, double scaling, int pageIndex, Action<IPageReader> action)
        {
            using (var docReader = _fixture.GetDocReader(type, filePath, password, scaling))
            using (var pageReader = docReader.GetPageReader(pageIndex))
            {
                action(pageReader);
            }
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, -1)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 19)]
        [InlineData(Input.FromFile, "Docs/protected_0.pdf", null, -3)]
        [InlineData(Input.FromFile, "Docs/protected_0.pdf", null, 10)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, -1)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 19)]
        [InlineData(Input.FromBytes, "Docs/protected_0.pdf", null, -3)]
        [InlineData(Input.FromBytes, "Docs/protected_0.pdf", null, 10)]
        public void GetPageReader_WhenCalledWithInvalidIndex_ShouldThrow(Input type, string filePath, string password, int pageIndex)
        {
            Assert.Throws<DocnetException>(() => ExecuteForDocument(type, filePath, password, 10, 10, pageIndex, _ => { }));
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 1, 1)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 10, 30)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 15, 40)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 1337, 1337)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 1997, 1000000)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 1, 1)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 10, 30)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 15, 40)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 1337, 1337)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 1997, 1000000)]
        public void GetPageWidthOrHeight_WhenCalled_ShouldMachOneOfTheDimensions(Input type, string filePath, string password, int dimOne, int dimTwo)
        {
            ExecuteForDocument(type, filePath, password, dimOne, dimTwo, 0, pageReader =>
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                var min = Math.Min(width, height);
                var max = Math.Max(width, height);

                Assert.True(min == dimOne || max == dimTwo);
                Assert.True(min <= dimOne && max <= dimTwo);
            });
        }

        [Theory]
        [InlineData(Input.FromFile)]
        [InlineData(Input.FromBytes)]
        public void GetPageWidthOrHeight_WhenCalled_ShouldKeepAspectRatio(Input type)
        {
            ExecuteForDocument(type, "Docs/simple_0.pdf", null, 500, 10000, 3, pageReader =>
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                Assert.Equal(1.414, (double)height / width, 3);
            });
        }

        [Theory]
        [InlineData(Input.FromFile)]
        [InlineData(Input.FromBytes)]
        public void PageIndex_WhenCalled_ShouldReturnCorrectIndex(Input type)
        {
            var random = new Random();

            var index = random.Next(19);

            ExecuteForDocument(type, "Docs/simple_0.pdf", null, 10, 10, index, pageReader =>
            {
                Assert.Equal(index, pageReader.PageIndex);
            });
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_2.pdf", 1, "2")]
        [InlineData(Input.FromFile, "Docs/simple_2.pdf", 3, "4 CONTENTS")]
        [InlineData(Input.FromFile, "Docs/simple_4.pdf", 0, "")]
        [InlineData(Input.FromFile, "Docs/simple_5.pdf", 0, "test.md 11/11/2018\r\n1 / 1\r\nTest document")]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", 1, "2")]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", 3, "4 CONTENTS")]
        [InlineData(Input.FromBytes, "Docs/simple_4.pdf", 0, "")]
        [InlineData(Input.FromBytes, "Docs/simple_5.pdf", 0, "test.md 11/11/2018\r\n1 / 1\r\nTest document")]
        public void GetText_WhenCalled_ShouldReturnValidText(Input type, string filePath, int pageIndex, string expectedText)
        {
            ExecuteForDocument(type, filePath, null, 10, 10, pageIndex, pageReader =>
            {
                var text = pageReader.GetText();

                Assert.Equal(expectedText, text);
            });
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_3.pdf", null, 1, "Simple PDF File 2")]
        [InlineData(Input.FromFile, "Docs/simple_3.pdf", null, 1, "Boring. More,")]
        [InlineData(Input.FromFile, "Docs/simple_3.pdf", null, 1, "The end, and just as well.")]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 4, "ASCIIHexDecode")]
        [InlineData(Input.FromFile, "Docs/protected_0.pdf", "password", 0, "The Secret (2016 film)")]
        [InlineData(Input.FromBytes, "Docs/simple_3.pdf", null, 1, "Simple PDF File 2")]
        [InlineData(Input.FromBytes, "Docs/simple_3.pdf", null, 1, "Boring. More,")]
        [InlineData(Input.FromBytes, "Docs/simple_3.pdf", null, 1, "The end, and just as well.")]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 4, "ASCIIHexDecode")]
        [InlineData(Input.FromBytes, "Docs/protected_0.pdf", "password", 0, "The Secret (2016 film)")]
        public void GetText_WhenCalled_ShouldContainValidText(Input type, string filePath, string password, int pageIndex, string expectedText)
        {
            ExecuteForDocument(type, filePath, password, 10, 10, pageIndex, pageReader =>
            {
                var text = pageReader.GetText();

                Assert.Contains(expectedText, text, StringComparison.Ordinal);
            });
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_2.pdf", null, 1, 1)]
        [InlineData(Input.FromFile, "Docs/simple_2.pdf", null, 3, 10)]
        [InlineData(Input.FromFile, "Docs/simple_5.pdf", null, 0, 40)]
        [InlineData(Input.FromFile, "Docs/protected_0.pdf", "password", 0, 2009)]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", null, 1, 1)]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", null, 3, 10)]
        [InlineData(Input.FromBytes, "Docs/simple_5.pdf", null, 0, 40)]
        [InlineData(Input.FromBytes, "Docs/protected_0.pdf", "password", 0, 2009)]
        public void GetCharacters_WhenCalled_ShouldReturnCharacters(Input type, string filePath, string password, int pageIndex, int charCount)
        {
            ExecuteForDocument(type, filePath, password, 10, 10, pageIndex, pageReader =>
            {
                var characters = pageReader.GetCharacters().ToArray();

                Assert.Equal(charCount, characters.Length);
            });
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_3.pdf", null, 1)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 18)]
        [InlineData(Input.FromFile, "Docs/protected_0.pdf", "password", 0)]
        [InlineData(Input.FromBytes, "Docs/simple_3.pdf", null, 1)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 18)]
        [InlineData(Input.FromBytes, "Docs/protected_0.pdf", "password", 0)]
        public void GetImage_WhenCalled_ShouldReturnNonZeroRawByteArray(Input type, string filePath, string password, int pageIndex)
        {
            ExecuteForDocument(type, filePath, password, 10, 10, pageIndex, pageReader =>
            {
                var bytes = pageReader.GetImage().ToArray();

                Assert.True(bytes.Length > 0);
                Assert.NotEmpty(bytes.Where(x => x != 0));
            });
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_3.pdf", null, 1)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 18)]
        [InlineData(Input.FromFile, "Docs/protected_0.pdf", "password", 0)]
        [InlineData(Input.FromBytes, "Docs/simple_3.pdf", null, 1)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", null, 18)]
        [InlineData(Input.FromBytes, "Docs/protected_0.pdf", "password", 0)]
        public void GetImageWithTransparentConverter_WhenCalled_ShouldReturnNonZeroRawByteArray(Input type, string filePath, string password, int pageIndex)
        {
            ExecuteForDocument(type, filePath, password, 1, pageIndex, pageReader =>
            {
                var bytes = pageReader.GetImage(new NaiveTransparencyRemover()).ToArray();

                Assert.True(bytes.Length > 0);
                Assert.NotEmpty(bytes.Where(x => x != 0));
            });
        }

        [Theory]
        [InlineData(Input.FromFile)]
        [InlineData(Input.FromBytes)]
        public void Reader_WhenCalledFromDifferentThreads_ShouldBeAbleToHandle(Input type)
        {
            var task1 = Task.Run(() => Assert.InRange(GetNonZeroByteCount(type, "Docs/simple_0.pdf", _fixture), 2000000, 2400000));
            var task2 = Task.Run(() => Assert.InRange(GetNonZeroByteCount(type, "Docs/simple_1.pdf", _fixture), 190000, 200000));
            var task3 = Task.Run(() => Assert.InRange(GetNonZeroByteCount(type, "Docs/simple_2.pdf", _fixture), 4500, 4900));
            var task4 = Task.Run(() => Assert.InRange(GetNonZeroByteCount(type, "Docs/simple_3.pdf", _fixture), 20000, 22000));
            var task5 = Task.Run(() => Assert.InRange(GetNonZeroByteCount(type, "Docs/simple_4.pdf", _fixture), 0, 0));

            Task.WaitAll(task1, task2, task3, task4, task5);
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 1, 595, 841)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 10, 5953, 8419)]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", null, 15, 8929, 12628)]
        public void GetPageWidthOrHeight_WhenCalledWithScalingFactor_ShouldMach(Input type, string filePath, string password, double scaling, int expectedWidth, int expectedHeight)
        {
            ExecuteForDocument(type, filePath, password, scaling, 0, pageReader =>
            {
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                Assert.Equal(expectedWidth, width);
                Assert.Equal(expectedHeight, height);
            });
        }

        [Fact]
        public void GetImage_WhenCalledWithRenderAnnotationsFlag_ShouldRenderAnnotation()
        {
            ExecuteForDocument(Input.FromFile, "Docs/annotation_0.pdf", null, 1, 0, pageReader =>
            {
                // verify pixel in center of image is the correct yellow color
                var bytes = pageReader.GetImage(RenderFlags.RenderAnnotations).ToArray();
                const int bpp = 4;
                var center = bytes.Length / bpp / 2 * bpp; // note integer division by 2 here.  we're getting the first byte in the central pixel
                Assert.Equal(133, bytes[center]); // Blue
                Assert.Equal(244, bytes[center + 1]); // Green
                Assert.Equal(252, bytes[center + 2]); // Red
                Assert.Equal(255, bytes[center + 3]); // Alpha
            });
        }

        [Fact]
        public void GetImage_WhenCalledWithRenderAnnotationsAndGrayscaleFlags_ShouldRenderAnnotationGrayscale()
        {
            ExecuteForDocument(Input.FromFile, "Docs/annotation_0.pdf", null, 1, 0, pageReader =>
            {
                // verify pixel in center of image is the correct gray color
                var bytes = pageReader.GetImage(RenderFlags.RenderAnnotations | RenderFlags.Grayscale).ToArray();
                const int bpp = 4;
                var center = bytes.Length / bpp / 2 * bpp; // note integer division by 2 here.  we're getting the first byte in the central pixel
                Assert.Equal(234, bytes[center]); // Blue
                Assert.Equal(234, bytes[center + 1]); // Green
                Assert.Equal(234, bytes[center + 2]); // Red
                Assert.Equal(255, bytes[center + 3]); // Alpha
            });
        }

        private static int GetNonZeroByteCount(Input type, string filePath, LibFixture fixture)
        {
            using (var reader = fixture.GetDocReader(type, filePath, null, 1000, 1000))
            {
                using (var pageReader = reader.GetPageReader(0))
                {
                    return pageReader.GetImage().Count(x => x != 0);
                }
            }
        }
    }
}
