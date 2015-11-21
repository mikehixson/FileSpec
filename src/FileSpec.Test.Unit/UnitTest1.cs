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
        public void BasicTest()
        {
            var package = CreatePackage();

            // Write
            DummyData data1 = new DummyData { Int32 = 123, String = "ABC", DateTime = new DateTime(1976, 04, 11)};
                      
            StringBuilder buffer = new StringBuilder();
            TextWriter writer = new StringWriter(buffer);

            Master master = new Master(typeof(DummyData), package);
            master.Write(data1, writer);

            // Read
            DelimitedParser prser = new DelimitedParser(new Reader(new MemoryStream(Encoding.UTF8.GetBytes(buffer.ToString()))), '*');
            //TextReader reader = new StringReader(buffer.ToString());
            DummyData data2 = master.Read<DummyData>(prser);
        }

        //public void EnumeratorTest()
        //{
        //    var package = CreatePackage();

        //    string data = "1*ABC*1/1/2000\r\n2*DEF*1/2/2000";
        //    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
        //    {
        //        var parser = new DelimitedParser(new Reader(stream), '*');

        //        var records = new MyEnumerator<DummyData>(parser, package);

        //        foreach (var record in records)
        //        {

        //        }
        //    }            
        //}

        private Package CreatePackage()
        {
            Package package = new Package
            {
                Writer = new DelimitedWriter(fieldDelimiter: "*"),
                //Reader = new DelimitedReader(fieldDelimiter: "*"),
                //Predicate = s => s == "A",
                Create = () => new DummyData(),
                Mappings = new List<IMapping>
                {
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<DummyData, int>(r => r.Int32), new NumberConverter()),
                        Field = new PositionedField(0)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<DummyData, string>(r => r.String), new StringConverter()),
                        Field = new PositionedField(1)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<DummyData, DateTime>(r => r.DateTime), new DateTimeConverter()),
                        Field = new PositionedField(2)
                    }
                }
            };

            return package;
        }


        private PropertyInfo GetProperty<T, R>(Expression<Func<T, R>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            return memberExpression.Member as PropertyInfo;
        }
    }
}
