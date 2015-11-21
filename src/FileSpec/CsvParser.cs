using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{        
    public class CsvParser : IParser
    {
        private readonly Reader _reader;
        private List<string> _current;

        // we are esentially providing random access to a parsed record.
        public List<string> Current
        {
            get { return _current; }
        }
                
        public CsvParser(Reader reader)
        {
            _reader = reader;
        }
                
        public bool Parse()
        {               
            if (_reader.EndOfStream())
                return false;

            _current = new List<string>(8); //todo: should we allow this to be specified?

            // 1: begining of field; 2: inside regular field; 3: inside quoted field; 4: end quote or escape           
            byte state = 1;

            while (true)
            {
                for (int i = _reader.Position; i < _reader.Length; i++)
                {
                    char c = _reader.Buffer[i];

                    if (state == 1) // 1: begining of field
                    {
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
                        else if (c == ',')
                        {
                            _current.Add(_reader.GetChars(i));
                            _reader.Position = i + 1;

                            state = 1;
                        }
                        else if (c == '"')
                        {
                            _reader.Position = i + 1;

                            state = 3;
                        }
                        else
                        {
                            state = 2;
                        }
                    }
                    else if (state == 2)     // 2: inside regular field
                    {
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
                        else if (c == ',')
                        {
                            _current.Add(_reader.GetChars(i));
                            _reader.Position = i + 1;

                            state = 1;
                        }
                    }
                    else if (state == 3)    // 3: inside quoted field
                    {
                        if (c == '"')
                        {
                            _reader.AppendChars(i);
                            _reader.Position = i + 1;

                            state = 4;
                        }
                    }
                    else if (state == 4)    // 4: end quote or escape
                    {
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
                        else if (c == ',')
                        {
                            _current.Add(_reader.GetChars(i));
                            _reader.Position = i + 1;

                            state = 1;
                        }
                        else
                        {
                            state = 3;
                        }
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
