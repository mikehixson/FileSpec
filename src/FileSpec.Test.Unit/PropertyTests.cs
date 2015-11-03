using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSpec.Converter;
using Xunit;

namespace FileSpec.Test.Unit
{
    public class PropertyTests
    {
        [Fact]
        public void Set_When_SetsProperty()
        {
            Property property = Create();
            DummyData data = new DummyData();

            property.Set(data, "123");

            Assert.Equal(123, data.Int32);
        }

        [Fact]
        public void Get_WhenCalled_GetsProperty()
        {
            Property property = Create();
            DummyData data = new DummyData { Int32 = 123 };

            var value = property.Get(data);

            Assert.Equal("123", value);
        }

        private Property Create()
        {
            var propertyInfo = typeof(DummyData).GetProperty("Int32");
            var propertyAccess = new NumberConverter();

            return new Property(propertyInfo, propertyAccess);
        }
    }
}
