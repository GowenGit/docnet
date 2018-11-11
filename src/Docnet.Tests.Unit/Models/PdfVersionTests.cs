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

        [Theory]
        [InlineData(10, "1.0")]
        [InlineData(11, "1.1")]
        [InlineData(12, "1.2")]
        [InlineData(13, "1.3")]
        [InlineData(14, "1.4")]
        [InlineData(15, "1.5")]
        [InlineData(16, "1.6")]
        [InlineData(17, "1.7")]
        [InlineData(20, "2.0")]
        public void ToString_WhenCalled_ShouldFormatString(int version, string str)
        {
            var sut = new PdfVersion(version);

            Assert.Equal(str, sut.ToString());
        }
    }
}
