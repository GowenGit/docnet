using System;
using Xunit;

namespace Docnet.Tests.Integration
{
    [Collection("Lib collection")]
    public sealed class MergeTests
    {
        private readonly LibFixture _fixture;

        public MergeTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Merge_WhenCalledWithFirstNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge(null, "dummy"));
        }

        [Fact]
        public void Merge_WhenCalledWithSecondNull_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge("dummy", null));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", "Docs/simple_1.pdf", 24)]
        [InlineData("Docs/simple_0.pdf", "Docs/simple_2.pdf", 29)]
        [InlineData("Docs/simple_1.pdf", "Docs/simple_3.pdf", 7)]
        public void Merge_WhenCalled_ShouldMergeTwoDocs(string fileOne, string fileTwo, int expectedPageCount)
        {
            var mergedBytes = _fixture.Lib.Merge(fileOne, fileTwo);

            using (var file = new TempFile(mergedBytes))
            {
                using (var reader = _fixture.Lib.GetDocReader(file.FilePath, 10, 10))
                {
                    Assert.Equal(expectedPageCount, reader.GetPageCount());
                }
            }
        }
    }
}