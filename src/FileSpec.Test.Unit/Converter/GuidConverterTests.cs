using FileSpec.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test.Converter
{
    public class GuidConverterTests
    {
        [Fact]
        public void GetString_NullFormat_IsValid()
        {
            GuidConverter converter = new GuidConverter();

            string result = converter.GetString(new Guid(new byte[16]));

            Assert.Equal("00000000-0000-0000-0000-000000000000", result);
        }

        [Fact]
        public void GetString_FormatSpecified_IsValid()
        {
            GuidConverter converter = new GuidConverter("N");

            string result = converter.GetString(new Guid(new byte[16]));

            Assert.Equal("00000000000000000000000000000000", result);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000000000000000000000000000")]
        [InlineData("{00000000-0000-0000-0000-000000000000}")]
        public void GetValue_NullFormat_IsValid(string value)
        {
            IConverter<Guid> converter = new GuidConverter();

            Guid result = converter.GetValue(value);

            Assert.Equal(new Guid(new byte[16]), result);
        }

        [Fact]
        public void GetValue_FormatSpecified_IsValid()
        {
            IConverter<Guid> converter = new GuidConverter("N");

            Guid result = converter.GetValue("00000000000000000000000000000000");

            Assert.Equal(new Guid(new byte[16]), result);
        }

        [Fact]
        public void GetValue_ValueDosentMatchFormat_ThrowsFormatException()
        {
            IConverter<Guid> converter = new GuidConverter("N");

            Action action = () => converter.GetValue("00000000-0000-0000-0000-000000000000");

            Assert.Throws<FormatException>(action);
        }
    }
}
