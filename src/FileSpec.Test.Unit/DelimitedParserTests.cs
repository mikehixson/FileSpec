using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test
{
    public class DelimitedParserTests
    {
        [Fact]
        public void Test()
        {
            string line = "ABC,123,DEFG,,H,\r\n1,2,3,,5,7";

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(line));
            stream.Position = 0;

            DelimitedParser parser = new DelimitedParser(new Reader(stream), ',');
            bool b = parser.Parse();

            Assert.True(b);
            Assert.Equal(new string[] { "ABC", "123", "DEFG", null, "H", null }, parser.Current);
        }
    }
}
