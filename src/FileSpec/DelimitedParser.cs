using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    // 0.3630
    public class DelimitedParser : IParser
    {        
        private readonly Reader _reader;
        private readonly char _fieldDelimiter;

        private List<string> _current;

        public List<string> Current
        {
            get { return _current; }
        }

        public DelimitedParser(Reader reader, char fieldDelimiter)
        {
            _reader = reader;
            _fieldDelimiter = fieldDelimiter;
        }

        public bool Parse()
        {           
            if (_reader.EndOfStream())
                return false;

            _current = new List<string>();

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
                    else if (c == _fieldDelimiter)
                    {
                        _current.Add(_reader.GetChars(i));
                        _reader.Position = i + 1;
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
