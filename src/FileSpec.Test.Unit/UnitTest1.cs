using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FileSpec.Converter;
using Xunit;

namespace FileSpec.Test.Unit
{
    public class UnitTest1
    {

        [Fact]
        public void Test()
        {
            Package package = new Package
            {
                Writer = new DelimitedWriter(fieldDelimiter: "*"),
                Reader = new DelimitedReader(fieldDelimiter: "*"),
                //Predicate = s => s == "A",
                Create = () => new DummyData(),
                Descriptions = new List<IMapping>
                {
                    new Mapping()
                    {
                        Property = new Property(GetProperty<DummyData, int>(r => r.Int32), new NumberConverter()),
                        Field = new PositionedField(0)
                    },
                    new Mapping()
                    {
                        Property = new Property(GetProperty<DummyData, string>(r => r.String), new StringConverter()),
                        Field = new PositionedField(1)
                    },
                    new Mapping()
                    {
                        Property = new Property(GetProperty<DummyData, DateTime>(r => r.DateTime), new DateTimeConverter()),
                        Field = new PositionedField(2)
                    }
                }
            };

            // Write
            DummyData data1 = new DummyData { Int32 = 123, String = "ABC", DateTime = new DateTime(1976, 04, 11)};
                      
            StringBuilder buffer = new StringBuilder();
            TextWriter writer = new StringWriter(buffer);

            Master master = new Master(typeof(DummyData), package);
            master.Write(data1, writer);

            // Read
            TextReader reader = new StringReader(buffer.ToString());
            DummyData data2 = master.Read<DummyData>(reader);
        }


        private PropertyInfo GetProperty<T, R>(Expression<Func<T, R>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            return memberExpression.Member as PropertyInfo;
        }
    }
}
