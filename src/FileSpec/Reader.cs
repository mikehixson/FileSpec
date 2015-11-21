using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class Reader
    {
        private StringBuilder _sb;
        private readonly BinaryReader _reader;

        // Fields are faster than properties
        public readonly char[] Buffer;
        public int Position;
        public int Length;

        public Reader(Stream stream)
            : this(stream, Encoding.UTF8, 1024)
        { }

        public Reader(Stream stream, Encoding encoding)
            : this(stream, encoding, 1024)
        { }

        public Reader(Stream stream, int bufferSize)
            : this(stream, Encoding.UTF8, bufferSize)
        { }

        public Reader(Stream stream, Encoding encoding, int bufferSize)
        {
            Buffer = new char[bufferSize];
            _reader = new BinaryReader(stream, encoding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EndOfStream()
        {
            return Position == Length && !ReadBuffer();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool DataAvailable()
        {
            return Position < Length || ReadBuffer();
        }

        // Returns chars from charPos upto i, not including i
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetChars(int i)
        {
            int length = i - Position;

            if (_sb != null)
            {
                _sb.Append(Buffer, Position, length);
                string s = _sb.ToString();

                _sb = null;
                return s;
            }
            else
            {
                if (length == 0)
                    return null;

                return new String(Buffer, Position, length);
            }
        }

        // Appends chars from charPos upto i, not including i
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendChars(int i)
        {
            int length = i - Position;

            if (length > 0)
            {
                if (_sb == null)
                    _sb = new StringBuilder(length + 80);

                _sb.Append(Buffer, Position, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBuffer()
        {
            Length = _reader.Read(Buffer, 0, Buffer.Length);
            Position = 0;

            return Length > 0;
        }
    }
}
