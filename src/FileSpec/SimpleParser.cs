using FileSpec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    // trying to read lines as fast as streamreader technique // Goal: 0.247
    // we are at 0.2464
    public class SimpleParser : IParser
    {
        private readonly Reader _reader;
        private List<string> _current;

        public List<string> Current
        {
            get { return _current; }
        }

        public SimpleParser(Reader reader)
        {
            _reader = reader;
        }

        public bool Parse()
        {
            if (_reader.EndOfStream())
                return false;

            _current = new List<string>(1);

            while (true)
            {
                for (int i = _reader.Position; i < _reader.Length; i++)
                {
                    char c = _reader.Buffer[i];

                    if (c == '\r' || c == '\n')
                    {
                        _current.Add(_reader.GetChars(i));
                        _reader.Position = i + 1;

                        if (c == '\r' && _reader.DataAvailable())
                        {
                            if (_reader.Buffer[_reader.Position] == '\n')
                                _reader.Position++;
                        }

                        return true;
                    }
                }

                // Buffer is going to be updated. Save what we need to.
                _reader.AppendChars(_reader.Length);

                if (!_reader.ReadBuffer())
                {
                    // End of stream
                    _current.Add(_reader.GetChars(0));
                    return true;
                }
            }
        }
    }
}
