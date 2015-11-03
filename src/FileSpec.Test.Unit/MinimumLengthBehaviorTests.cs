using FileSpec.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test.Unit
{
    public class MinimumLengthBehaviorTests
    {

        [Theory]
        [InlineData("123", "123  ")]
        [InlineData("12345", "12345")]
        [InlineData("123456", "123456")]
        [InlineData(null, "     ")]
        public void Write_LeftAlignmentLength5_PadsToCorrectLength(string test, string expected)
        {
            MinimumLengthBehavior behavior = new MinimumLengthBehavior(5, FieldAlign.Left);

            var result = behavior.Write(test);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123", "  123")]
        [InlineData("12345", "12345")]
        [InlineData("123456", "123456")]
        [InlineData(null, "     ")]
        public void Write_RightAlignment_PadsToCorrectLength(string test, string expected)
        {
            MinimumLengthBehavior behavior = new MinimumLengthBehavior(5, FieldAlign.Right);

            var result = behavior.Write(test);

            Assert.Equal(expected, result);
        }
    }
}
