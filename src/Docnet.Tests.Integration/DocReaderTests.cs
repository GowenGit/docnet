using System;
using Docnet.Core.Exceptions;
using Xunit;

namespace Docnet.Tests.Integration
{
    [Collection("Lib collection")]
    public sealed class DocReaderTests
    {
        private readonly LibFixture _fixture;

        public DocReaderTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 1)]
        [InlineData(-1, 3)]
        [InlineData(0, -10)]
        [InlineData(-20, -10)]
        public void GetDocReader_WhenCalledWithInvalidDimensions_ShouldThrow(int dimOne, int dimTwo)
        {
            Assert.Throws<ArgumentException>(() => _fixture.Lib.GetDocReader("Docs/simple_0.pdf", null, dimOne, dimTwo));
        }

        [Fact]
        public void GetDocReader_WhenCalledWithInvalidFileData_ShouldThrow()
        {
            Assert.Throws<DocnetException>(() => _fixture.Lib.GetDocReader("Docs/protected_0.pdf", null, 10, 10));
        }

        [Fact]
        public void GetDocReader_WhenCalledWithNullFilePath_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.GetDocReader(null, null, 10, 10));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", null, 19)]
        [InlineData("Docs/simple_1.pdf", null, 5)]
        [InlineData("Docs/simple_2.pdf", null, 10)]
        [InlineData("Docs/simple_3.pdf", null, 2)]
        [InlineData("Docs/protected_0.pdf", "password", 3)]
        public void GetPageCount_WhenCalled_ShouldReturnCorrectResults(string filePath, string password, int expectedCount)
        {
            using (var reader = _fixture.Lib.GetDocReader(filePath, password, 10, 10))
            {
                var count = reader.GetPageCount();

                Assert.Equal(expectedCount, count);
            }
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", null, 17)]
        [InlineData("Docs/simple_1.pdf", null, 13)]
        [InlineData("Docs/simple_2.pdf", null, 12)]
        [InlineData("Docs/simple_3.pdf", null, 13)]
        [InlineData("Docs/protected_0.pdf", "password", 17)]
        public void GetPdfVersion_WhenCalled_ShouldReturnCorrectResults(string filePath, string password, int expectedVersion)
        {
            using (var reader = _fixture.Lib.GetDocReader(filePath, password, 10, 10))
            {
                var version = reader.GetPdfVersion();

                Assert.Equal(expectedVersion, version.Number);
            }
        }
    }
}