using System;
using Docnet.Core.Models;
using Xunit;

namespace Docnet.Tests.Unit.Models
{
    public class PdfVersionTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(18)]
        [InlineData(9)]
        [InlineData(21)]
        [InlineData(-1)]
        [InlineData(0)]
        public void Ctor_WhenCalledWithInvalidVersion_ShouldThrow(int version)
        {
            Assert.Throws<ArgumentException>(() => new PdfVersion(version));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(20)]
        public void Ctor_WhenCalledWithValidVersion_ShouldCreateInstance(int version)
        {
            var sut = new PdfVersion(version);

            Assert.Equal(version, sut.Number);
        }
    }
}
