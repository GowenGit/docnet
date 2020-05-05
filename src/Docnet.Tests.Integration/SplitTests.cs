using System;
using System.IO;
using Docnet.Core.Exceptions;
using Docnet.Core.Models;
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
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split((string)null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split((string)null, string.Empty));
        }

        [Fact]
        public void Split_WhenCalledWithNullBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split((byte[])null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Split((byte[])null, string.Empty));
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
        [InlineData(Input.FromFile, "Docs/simple_0.pdf", "1-21")]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", "1-21")]
        [InlineData(Input.FromFile, "Docs/simple_1.pdf", "3-8")]
        [InlineData(Input.FromBytes, "Docs/simple_1.pdf", "3-8")]
        public void Split_WhenStringRangeToBig_ShouldThrow(Input type, string filePath, string pageRange)
        {
            Assert.Throws<DocnetException>(() => _fixture.Split(type, filePath, pageRange));
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
                using (var reader = _fixture.Lib.GetDocReader(file.FilePath, new PageDimensions(10, 10)))
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

            using (var reader = _fixture.Lib.GetDocReader(splitBytes, new PageDimensions(10, 10)))
            {
                Assert.Equal(expectedPageCount, reader.GetPageCount());
            }
        }

        [Theory]
        [InlineData(Input.FromFile,  "Docs/simple_0.pdf", "1-19", 19)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", "1-19", 19)]
        [InlineData(Input.FromFile,  "Docs/simple_2.pdf", "1-2", 2)]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", "1-2", 2)]
        [InlineData(Input.FromFile,  "Docs/simple_0.pdf", "18", 1)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", "18", 1)]
        [InlineData(Input.FromFile,  "Docs/simple_0.pdf", "1-7,12,14-18", 13)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", "1-7,12,14-18", 13)]
        [InlineData(Input.FromFile,  "Docs/simple_0.pdf", "1 - 7,  12, 14-18", 13)]
        [InlineData(Input.FromBytes, "Docs/simple_0.pdf", "1 - 7,  12, 14-18", 13)]
        [InlineData(Input.FromFile,  "Docs/simple_2.pdf", "1-3,6-9", 7)]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", "1-3,6-9", 7)]
        [InlineData(Input.FromFile,  "Docs/simple_2.pdf", "6-9, 4, 1-3", 8)]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", "6-9, 4, 1-3", 8)]
        [InlineData(Input.FromFile,  "Docs/simple_2.pdf", " 1,1,1", 3)]
        [InlineData(Input.FromBytes, "Docs/simple_2.pdf", " 1,1,1", 3)]
        [InlineData(Input.FromFile,  "Docs/simple_3.pdf", "  1 - 1  ", 1)]
        [InlineData(Input.FromBytes, "Docs/simple_3.pdf", "  1 - 1  ", 1)]
        public void Split_WhenCalledWithValidStringRange_ShouldSplitDoc(Input type, string filePath, string pageRange, int expectedPageCount)
        {
            var splitBytes = _fixture.Split(type, filePath, pageRange);

            using (var reader = _fixture.Lib.GetDocReader(splitBytes, new PageDimensions(10, 10)))
            {
                Assert.Equal(expectedPageCount, reader.GetPageCount());
            }
        }
    }
}
