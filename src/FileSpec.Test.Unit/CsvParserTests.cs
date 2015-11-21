using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSpec.Test
{    
    public class CsvParserTests
    {
        [Theory]
        [InlineData("aaa,bbb", new string[] { "aaa", "bbb" })]
        [InlineData("\"aaa\",\"bbb\"", new string[] { "aaa", "bbb" })]
        [InlineData(",", new string[] { null, null })]
        public void Read_EOFLineEnding_SingleCompleteRecord(string line, string[] expected)
        {
            CsvParser parser = Create(line);

            bool b1 = parser.Parse();
            var record1 = parser.Current;

            bool b2 = parser.Parse();

            Assert.Equal(expected, record1);
            Assert.False(b2);
        }

        [Theory]
        [InlineData("aaa,bbb", new string[] { "aaa", "bbb" })]
        [InlineData("\"aaa\",\"bbb\"", new string[] { "aaa", "bbb" })]
        [InlineData(",", new string[] { null, null })]
        public void Read_CRLFLineEnding_SingleCompleteRecord(string line, string[] expected)
        {
            CsvParser parser = Create(line + "\r\n");

            bool b1 = parser.Parse();
            var record1 = parser.Current;

            bool b2 = parser.Parse();

            Assert.Equal(expected, record1);
            Assert.False(b2);
        }

        [Theory]
        [InlineData("aaa,bbb", new string[] { "aaa", "bbb" })]
        [InlineData("\"aaa\",\"bbb\"", new string[] { "aaa", "bbb" })]
        [InlineData(",", new string[] { null, null })]
        public void Read_LFLineEnding_SingleCompleteRecord(string line, string[] expected)
        {
            CsvParser parser = Create(line + "\n");

            bool b1 = parser.Parse();
            var record1 = parser.Current;

            bool b2 = parser.Parse();

            Assert.Equal(expected, record1);
            Assert.False(b2);
        }

        [Theory]
        [InlineData("aaa,bbb", new string[] { "aaa", "bbb" })]
        [InlineData("\"aaa\",\"bbb\"", new string[] { "aaa", "bbb" })]
        [InlineData(",", new string[] { null, null })]
        public void Read_CRLineEnding_SingleCompleteRecord(string line, string[] expected)
        {
            CsvParser parser = Create(line + "\r");

            bool b1 = parser.Parse();
            var record1 = parser.Current;

            bool b2 = parser.Parse();

            Assert.Equal(expected, record1);
            Assert.False(b2);
        }

        [Theory]
        [InlineData("\"aaa\",bbb,ccc", 0, "aaa")]
        [InlineData("aaa,\"bbb\",ccc", 1, "bbb")]
        [InlineData("aaa,bbb,\"ccc\"", 2, "ccc")]
        public void Read_QuotedField_ValueCorrect(string line, int field, string expected)
        {
            CsvParser parser = Create(line);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(expected, record[field]);
        }

        [Theory]
        [InlineData("\"a\"\"aa\",bbb,ccc", 0, "a\"aa")]
        [InlineData("aaa,\"b\"\"bb\",ccc", 1, "b\"bb")]
        [InlineData("aaa,bbb,\"c\"\"cc\"", 2, "c\"cc")]
        public void Read_QuotedFieldContainsDoubleQuote_ValueCorrect(string line, int field, string expected)
        {
            CsvParser parser = Create(line);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(expected, record[field]);
        }

        [Theory]
        [InlineData("\"a,aa\",bbb,ccc", 0, "a,aa")]
        [InlineData("aaa,\"b,bb\",ccc", 1, "b,bb")]
        [InlineData("aaa,bbb,\"c,cc\"", 2, "c,cc")]
        public void Read_QuotedFieldContainsComma_ValueCorrect(string line, int field, string expected)
        {
            CsvParser parser = Create(line);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(expected, record[field]);
        }

        [Theory]
        [InlineData("\"a\r\naa\",bbb,ccc", 0, "a\r\naa")]
        [InlineData("aaa,\"b\r\nbb\",ccc", 1, "b\r\nbb")]
        [InlineData("aaa,bbb,\"c\r\ncc\"", 2, "c\r\ncc")]
        public void Read_QuotedFieldContainsCRLF_ValueCorrect(string line, int field, string expected)
        {
            CsvParser parser = Create(line);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(expected, record[field]);
        }

        [Theory]
        [InlineData("aa\"a,bbb,ccc", 0, "aa\"a")]
        [InlineData("aaa,bb\"b,ccc", 1, "bb\"b")]
        [InlineData("aaa,bbb,cc\"c", 2, "cc\"c")]
        public void Read_FieldContainsDoubleQuote_ValueCorrect(string line, int field, string expected)
        {
            CsvParser parser = Create(line);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(expected, record[field]);
        }


        [Theory]
        [InlineData(2)]     //aa^a,^bb^b,^cc^c
        [InlineData(6)]     //aaa,bb^b,ccc
        [InlineData(10)]    //aaa,bbb,cc^c
        public void Read_FieldSpansBuffers_RecordComplete(int bufferSize)
        {
            CsvParser parser = Create("aaa,bbb,ccc", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Theory]
        [InlineData(3)]     //"aa^a",^"bb^b",^"cc^c"
        [InlineData(9)]     //"aaa","bb^b","ccc"
        [InlineData(15)]    //"aaa","bbb","cc^c"
        public void Read_QuotedFieldSpansBuffers_RecordComplete(int bufferSize)
        {
            CsvParser parser = Create("\"aaa\",\"bbb\",\"ccc\"", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Theory]
        [InlineData(5)]     //"aaa"^,"bbb^","cc^c"
        [InlineData(11)]    //"aaa","bbb"^,"ccc"
        [InlineData(17)]    //"aaa","bbb","ccc"
        public void Read_EndingDoubleQuouteLastInBuffer_RecordComplete(int bufferSize)
        {
            CsvParser parser = Create("aaa,bbb,ccc", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Theory]
        [InlineData(4)]     //"aaa^","b^bb",^"ccc^"
        [InlineData(10)]    //"aaa","bbb^","ccc"
        [InlineData(16)]    //"aaa","bbb","ccc^"
        public void Read_EndingDoubleQuoteFirstInBuffer_RecordComplete(int bufferSize)
        {
            CsvParser parser = Create("aaa,bbb,ccc", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Theory]
        [InlineData(3)]     //"aa^""a^","^bb"^"b"^,"c^c""^c"
        [InlineData(11)]    //"aa""a","bb^""b","cc""c^"
        [InlineData(19)]    //"aa""a","bb""b","cc^""c"
        public void Read_EscapingQuoteFirstInBuffer(int bufferSize)
        {
            CsvParser parser = Create("\"aa\"\"a\",\"bb\"\"b\",\"cc\"\"c\"", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aa\"a", "bb\"b", "cc\"c" }, record);
        }

        [Theory]
        [InlineData(4)]     //"aa"^"a",^"bb"^"b",^"cc"^"c"
        [InlineData(12)]    //"aa""a","bb"^"b","cc""c"
        [InlineData(20)]    //"aa""a","bb""b","cc"^"c"
        public void Read_EscapingQuoteLastInBuffer(int bufferSize)
        {
            CsvParser parser = Create("\"aa\"\"a\",\"bb\"\"b\",\"cc\"\"c\"", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aa\"a", "bb\"b", "cc\"c" }, record);
        }

        [Theory]
        [InlineData(4)]     //aaa,^bbb,^ccc
        [InlineData(8)]     //aaa,bbb,^ccc
        public void Read_CommaLastInBuffer_RecordComplete(int bufferSize)
        {
            CsvParser parser = Create("aaa,bbb,ccc", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Theory]
        [InlineData(3)]     //aaa^,bb^b,c^cc
        [InlineData(7)]     //aaa,bbb^,ccc
        public void Read_CommaFirstInBuffer_RecordComplete(int bufferSize)
        {
            CsvParser parser = Create("aaa,bbb,ccc", bufferSize);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Fact]
        public void Read_CRLFLastInBuffer_RecordComplete()
        {
            CsvParser parser = Create("aaa,bbb,ccc\r\n", 13);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Fact]
        public void Read_CRLFFirstInBuffer_RecordComplete()
        {
            CsvParser parser = Create("aaa,bbb,ccc\r\n", 11);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        [Fact]
        public void Read_CRLFSpansBuffers_RecordComplete()
        {
            CsvParser parser = Create("aaa,bbb,ccc\r\n", 12);

            bool b1 = parser.Parse();
            var record = parser.Current;

            Assert.Equal(new string[] { "aaa", "bbb", "ccc" }, record);
        }

        private CsvParser Create(string text)
        {
            return new CsvParser(new Reader(new MemoryStream(Encoding.UTF8.GetBytes(text))));
        }

        private CsvParser Create(string text, int bufferSize)
        {
            return new CsvParser(new Reader(new MemoryStream(Encoding.UTF8.GetBytes(text)), bufferSize));            
        }
    }
}
