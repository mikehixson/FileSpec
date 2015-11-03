using FileSpec.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test.Behavior
{
    public class MinimumLengthBehaviorTests
    {
        [Theory]
        [InlineData("123", "123  ")]
        [InlineData("12345", "12345")]
        [InlineData("123456", "123456")]
        [InlineData(null, "     ")]
        public void Write_RightPadding_PadsToCorrectLength(string test, string expected)
        {
            MinimumLengthBehavior behavior = new MinimumLengthBehavior(5, FieldPadding.Right);

            var result = behavior.Write(test);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123", "  123")]
        [InlineData("12345", "12345")]
        [InlineData("123456", "123456")]
        [InlineData(null, "     ")]
        public void Write_LeftPadding_PadsToCorrectLength(string test, string expected)
        {
            MinimumLengthBehavior behavior = new MinimumLengthBehavior(5, FieldPadding.Left);

            var result = behavior.Write(test);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123  ", "123")]
        [InlineData(" 123 ", " 123")]
        [InlineData("12345", "12345")]
        [InlineData("123456", "123456")]
        [InlineData(null, null)]
        public void Read_RightPadding_RemovesRightPadding(string test, string expected)
        {
            MinimumLengthBehavior behavior = new MinimumLengthBehavior(5, FieldPadding.Right);

            var result = behavior.Read(test);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("  123", "123")]
        [InlineData(" 123 ", "123 ")]
        [InlineData("12345", "12345")]
        [InlineData("123456", "123456")]
        [InlineData(null, null)]
        public void Read_LeftPadding_RemovesLeftPadding(string test, string expected)
        {
            MinimumLengthBehavior behavior = new MinimumLengthBehavior(5, FieldPadding.Left);

            var result = behavior.Read(test);

            Assert.Equal(expected, result);
        }
    }
}
