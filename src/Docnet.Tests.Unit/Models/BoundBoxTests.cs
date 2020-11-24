using System;
using Docnet.Core.Models;
using Xunit;

namespace Docnet.Tests.Unit.Models
{
    public class BoundBoxTests
    {
        // test removed to allow bounding boxes with origin bottom left
        /*
        [theory]
        [inlinedata(-1, 0, 0, 0)]
        [inlinedata(0, -1, 0, 0)]
        [inlinedata(0, 0, -1, 0)]
        [inlinedata(0, 0, 0, -1)]
        [inlinedata(5, 0, 2, 0)]
        [inlinedata(0, 5, 0, 2)]
        public void ctor_whencalledwithinvalidversion_shouldthrow(int left, int top, int right, int bottom)
        {
            assert.throws<argumentexception>(() => new boundbox(left, top, right, bottom));
        }
        */

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