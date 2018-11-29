using System;
using System.IO;
using Docnet.Core.Exceptions;
using Docnet.Tests.Integration.Utils;
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
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split((string) null, 0, 0));
        }

        [Fact]
        public void Split_WhenCalledWithNullBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split((byte[])null, 0, 0));
        }

        [Theory]
        [InlineData(Input.FromFile, -1, 0)]
        [InlineData(Input.FromBytes, -1, 0)]
        [InlineData(Input.FromFile, 2, 1)]
        [InlineData(Input.FromBytes, 2, 1)]
        [InlineData(Input.FromFile, 0, -20)]
        [InlineData(Input.FromBytes, 0, -20)]
        [InlineData(Input.FromFile, 3, -20)]
        [InlineData(Input.FromBytes, 3, -20)]
        public void Split_WhenCalledWithInvalidIndex_ShouldThrow(Input type, int fromIndex, int toIndex)
        {
            Assert.Throws<ArgumentException>(() => _fixture.Split(type, "Docs/simple_0.pdf", fromIndex, toIndex));
        }

        [Theory]
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", 0, 20)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", 0, 20)]
        [InlineData(Input.FromFile, "Docs/simple_1.pdf", 2, 7)]
        [InlineData(Input.FromBytes, "Docs/simple_1.pdf", 2, 7)]
        public void Split_WhenRangeToBig_ShouldThrow(Input type, string filePath, int fromIndex, int toIndex)
        {
            Assert.Throws<DocnetException>(() => _fixture.Split(type, filePath, fromIndex, toIndex));
        }

        [Theory]
        [InlineData("Docs/simple_0.pdf", 0, 18, 19)]
        [InlineData("Docs/simple_0.pdf", 0, 0, 1)]
        [InlineData("Docs/simple_0.pdf", 18, 18, 1)]
        [InlineData("Docs/simple_2.pdf", 5, 9, 5)]
        public void Split_WhenCalledWithPaths_ShouldSplitDoc(string filePath, int fromIndex, int toIndex, int expectedPageCount)
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

        [Theory]
        [InlineData("Docs/simple_0.pdf", 0, 18, 19)]
        [InlineData("Docs/simple_0.pdf", 0, 0, 1)]
        [InlineData("Docs/simple_0.pdf", 18, 18, 1)]
        [InlineData("Docs/simple_2.pdf", 5, 9, 5)]
        public void Split_WhenCalledWithBytes_ShouldSplitDoc(string filePath, int fromIndex, int toIndex, int expectedPageCount)
        {
            var bytes = File.ReadAllBytes(filePath);

            var splitBytes = _fixture.Lib.Split(bytes, fromIndex, toIndex);

            using (var reader = _fixture.Lib.GetDocReader(splitBytes, 10, 10))
            {
                Assert.Equal(expectedPageCount, reader.GetPageCount());
            }
        }
    }
}
