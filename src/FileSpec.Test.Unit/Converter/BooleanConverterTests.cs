using FileSpec.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test.Converter
{
    public class BooleanConverterTests
    {
        [Fact]
        public void GetString_WhenCalled_IsValid()
        {
            BooleanConverter converter = new BooleanConverter("T", "F");

            var trueResult = converter.GetString(true);
            var falseResult = converter.GetString(false);

            Assert.Equal("T", trueResult);
            Assert.Equal("F", falseResult);
        }

        [Fact]
        public void GetValue_WhenCalled_IsValid()
        {
            BooleanConverter converter = new BooleanConverter("T", "F");

            var trueResult = converter.GetValue("T");
            var falseResult = converter.GetValue("F");

            Assert.True(trueResult);
            Assert.False(falseResult);
        }

        [Fact]
        public void GetValue_InalidValue_ThrowsApplicationException()
        {
            BooleanConverter converter = new BooleanConverter("T", "F");

            Action action = ()=> converter.GetValue("True");
            
            Assert.Throws<ApplicationException>(action);
            
        }
    }
}
