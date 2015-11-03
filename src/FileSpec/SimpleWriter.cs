using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class SimpleWriter : IRecordWriter
    {
        private readonly string _recordDelimiter;
        private StringBuilder _buffer;

        public SimpleWriter(string recordDelimiter = "\r\n")
        {
            _recordDelimiter = recordDelimiter;

            _buffer = new StringBuilder();
        }

        public void WriteStartRecord(TextWriter writer)
        {
            _buffer.Clear();
        }

        public void Write(string value, int delimitedContext)
        {
            if (delimitedContext != 0)
                throw new ArgumentOutOfRangeException("delimitedContext");

            _buffer.Append(value);
        }

        public void Write(string value, int delimitedContext, int index)
        {
            if (delimitedContext != 0)
                throw new ArgumentOutOfRangeException("delimitedContext");

            if (_buffer.Length < index)
                _buffer.Append(' ', index - _buffer.Length);
            else if (_buffer.Length > index)
                throw new ApplicationException("Cant write value at specified index."); //shouldn't happen if we write in index order

            _buffer.Append(value);
        }

        public void WriteEndRecord(TextWriter writer)   // hacked in
        {
            writer.Write(_buffer);
            writer.Write(_recordDelimiter);
        }
    }
}
