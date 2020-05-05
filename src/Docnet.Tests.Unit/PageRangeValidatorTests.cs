using System;
using Docnet.Core.Validation;
using Xunit;

namespace Docnet.Tests.Unit
{
    public class PageRangeValidatorTests
    {
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        [InlineData(2, 1)]
        [InlineData(0, -20)]
        [InlineData(3, -20)]
        public void Split_WhenCalledWithInvalidIndex_ShouldThrow(int fromIndex, int toIndex)
        {
            Assert.Throws<ArgumentException>(() => Validator.ValidatePageIndices(fromIndex, toIndex));
        }

        [Theory]
        [InlineData("0,0,0")]
        [InlineData("0-1")]
        [InlineData("1-5,0")]
        [InlineData("0-0")]
        [InlineData("0-0,0")]
        public void Split_WhenOneOfPagesZeroBased_ShouldThrow(string pageRange)
        {
            Assert.Throws<ArgumentException>(() => Validator.ValidatePageNumbers(pageRange, nameof(pageRange)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(",,")]
        [InlineData(" ,, ")]
        [InlineData("1,")]
        [InlineData("1,,,")]
        [InlineData("1, ,")]
        [InlineData("1,-")]
        [InlineData("1,-, ")]
        [InlineData("1,2 -")]
        [InlineData("1,2-abcd")]
        [InlineData("1,2-5,xyz")]
        [InlineData("1,3-d7")]
        [InlineData("1, asd3 - d7, 9")]
        [InlineData("1,7d-2")]
        [InlineData("1d, 7-9")]
        [InlineData("a, b-c ,d")]
        [InlineData("1, 2 5-27 ,29")]
        [InlineData("1 3")]
        [InlineData("1 3, 14")]
        [InlineData("-1, 8")]
        [InlineData("-1")]
        [InlineData("2-2- 2")]
        [InlineData(" 2-3-7")]
        [InlineData("2-5 - 14- 18")]
        public void Split_WhenCalledWithInvalidRange_ShouldThrow(string pageRange)
        {
            Assert.Throws<ArgumentException>(() => Validator.ValidatePageNumbers(pageRange, nameof(pageRange)));
        }
    }
}