using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test
{
    public class SimpleParserTests
    {
        private char[] _test;
        private MemoryStream _stream;

        [Fact]
        public void Test()
        {
            SimpleParser parser = Setup();
            bool b = parser.Parse();

            Assert.True(b);
            Assert.Equal(new String(_test), parser.Current[0]);            
        }

        private SimpleParser Setup()
        {
            _test = Enumerable.Range(65, 26).Select(i => Convert.ToChar(i)).ToArray();

            _stream = new MemoryStream();

            for(int i = 0; i < 5; i++)
            {
                _stream.Write(_test.Skip(i).Select(c => (byte)c).ToArray(), 0, 26 - i);
                _stream.Write(Encoding.ASCII.GetBytes("\r\n"), 0, 2);
            }

            _stream.Position = 0;

            return new SimpleParser(new Reader(_stream));
        }

    }
}
