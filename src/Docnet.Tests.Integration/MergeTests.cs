using System;
using System.IO;
using Docnet.Core.Models;
using Docnet.Tests.Integration.Utils;
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
        public void Merge_WhenCalledWithFirstNullString_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge(null, "dummy"));
        }

        [Fact]
        public void Merge_WhenCalledWithFirstNullBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge(null, new byte[1]));
        }

        [Fact]
        public void Merge_WhenCalledWithFirstEmptyBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge(Array.Empty<byte>(), new byte[1]));
        }

        [Fact]
        public void Merge_WhenCalledWithSecondNullString_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge("dummy", null));
        }

        [Fact]
        public void Merge_WhenCalledWithSecondNullBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge(new byte[1], null));
        }

        [Fact]
        public void Merge_WhenCalledWithSecondEmptyBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Merge(new byte[1], Array.Empty<byte>()));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", "Docs/simple_1.pdf", 24)]
        [InlineData("Docs/simple_0.pdf", "Docs/simple_2.pdf", 29)]
        [InlineData("Docs/simple_1.pdf", "Docs/simple_3.pdf", 7)]
        public void Merge_WhenCalledWithPaths_ShouldMergeTwoDocs(string fileOne, string fileTwo, int expectedPageCount)
        {
            var mergedBytes = _fixture.Lib.Merge(fileOne, fileTwo);

            using (var file = new TempFile(mergedBytes))
            {
                using (var reader = _fixture.Lib.GetDocReader(file.FilePath, new PageDimensions(10, 10)))
                {
                    Assert.Equal(expectedPageCount, reader.GetPageCount());
                }
            }
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", "Docs/simple_1.pdf", 24)]
        [InlineData("Docs/simple_0.pdf", "Docs/simple_2.pdf", 29)]
        [InlineData("Docs/simple_1.pdf", "Docs/simple_3.pdf", 7)]
        public void Merge_WhenCalledWithBytes_ShouldMergeTwoDocs(string fileOne, string fileTwo, int expectedPageCount)
        {
            var bytesOne = File.ReadAllBytes(fileOne);
            var bytesTwo = File.ReadAllBytes(fileTwo);

            var mergedBytes = _fixture.Lib.Merge(bytesOne, bytesTwo);

            using (var reader = _fixture.Lib.GetDocReader(mergedBytes, new PageDimensions(10, 10)))
            {
                Assert.Equal(expectedPageCount, reader.GetPageCount());
            }
        }
    }
}