using System;
using Docnet.Core.Models;
using Xunit;

namespace Docnet.Tests.Unit.Models
{
    public class BoundBoxTests
    {
        [Theory]
        [InlineData(-1, 0, 0, 0)]
        [InlineData(0, -1, 0, 0)]
        [InlineData(0, 0, -1, 0)]
        [InlineData(0, 0, 0, -1)]
        [InlineData(5, 0, 2, 0)]
        [InlineData(0, 5, 0, 2)]
        public void Ctor_WhenCalledWithInvalidVersion_ShouldThrow(int left, int top, int right, int bottom)
        {
            Assert.Throws<ArgumentException>(() => new BoundBox(left, top, right, bottom));
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0, 0, 2, 3)]
        [InlineData(12, 34, 266, 1337)]
        public void Ctor_WhenCalledWithValidVersion_ShouldCreateInstance(int left, int top, int right, int bottom)
        {
            var sut = new BoundBox(left, top, right, bottom);

            Assert.Equal(left, sut.Left);
            Assert.Equal(top, sut.Top);
            Assert.Equal(right, sut.Right);
            Assert.Equal(bottom, sut.Bottom);
        }
    }
}