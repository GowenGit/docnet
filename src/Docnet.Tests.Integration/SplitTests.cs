using System;
using Docnet.Core.Exceptions;
using Xunit;

namespace Docnet.Tests.Integration
{
    [Collection("Lib collection")]
    public sealed class SplitTests
    {
        private readonly LibFixture _fixture;

        public SplitTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Split_WhenCalledWithNullFilePath_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split(null, 0, 0));
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(2, 1)]
        [InlineData(0, -20)]
        [InlineData(3, -20)]
        public void Split_WhenCalledWithInvalidIndex_ShouldThrow(int fromIndex, int toIndex)
        {
            Assert.Throws<ArgumentException>(() => _fixture.Lib.Split("dummy", fromIndex, toIndex));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", 0, 20)]
        [InlineData("Docs/simple_1.pdf", 2, 7)]
        public void Split_WhenRangeToBig_ShouldThrow(string filePath, int fromIndex, int toIndex)
        {
            Assert.Throws<DocnetException>(() => _fixture.Lib.Split(filePath, fromIndex, toIndex));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", 0, 18, 19)]
        [InlineData("Docs/simple_0.pdf", 0, 0, 1)]
        [InlineData("Docs/simple_0.pdf", 18, 18, 1)]
        [InlineData("Docs/simple_2.pdf", 5, 9, 5)]
        public void Split_WhenCalled_ShouldSplitDoc(string filePath, int fromIndex, int toIndex, int expectedPageCount)
        {
            var splitBytes = _fixture.Lib.Split(filePath, fromIndex, toIndex);

            using (var file = new TempFile(splitBytes))
            {
                using (var reader = _fixture.Lib.GetDocReader(file.FilePath, 10, 10))
                {
                    Assert.Equal(expectedPageCount, reader.GetPageCount());
                }
            }
        }
    }
}
