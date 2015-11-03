using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class DelimitedWriter : IRecordWriter
    {
        private readonly string _fieldDelimiter;
        private readonly string _recordDelimiter;
        private List<StringBuilder> _buffers;

        public DelimitedWriter(string fieldDelimiter = ",", string recordDelimiter = "\r\n")
        {
            _fieldDelimiter = fieldDelimiter;
            _recordDelimiter = recordDelimiter;
            _buffers = new List<StringBuilder>();
        }

        public void WriteStartRecord(TextWriter writer)
        {
            ClearBuffers();
        }

        public void Write(string value, int delimitedContext)
        {
            StringBuilder buffer = GetBuffer(delimitedContext);
            buffer.Append(value);
        }

        public void Write(string value, int delimitedContext, int index)
        {
            StringBuilder buffer = GetBuffer(delimitedContext);

            if (buffer.Length < index)
                buffer.Append(' ', index - buffer.Length);
            else if (buffer.Length > index)
                throw new ApplicationException("Cant write value at specified index."); //shouldn't happen if we write in index order

            buffer.Append(value);
        }
        
        public void WriteEndRecord(TextWriter writer) 
        {
            WriteBuffers(writer);
        }

        private void ClearBuffers()
        {
            foreach (StringBuilder buffer in _buffers)
                buffer.Clear();
        }

        private void WriteBuffers(TextWriter writer)
        {
            bool hasData = false;

            foreach (StringBuilder buffer in _buffers)
            {
                if (hasData)
                    writer.Write(_fieldDelimiter);

                writer.Write(buffer);

                hasData = true;
            }

            writer.Write(_recordDelimiter);
        }

        private StringBuilder GetBuffer(int delimitedContext)
        {
            while (_buffers.Count <= delimitedContext)
                _buffers.Add(new StringBuilder());

            return _buffers[delimitedContext];
        }
    }
}
