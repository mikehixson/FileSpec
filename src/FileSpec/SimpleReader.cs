using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    // maybe this guy just always gives the same string when asked for read, the other reader will return the string from parts[]..
    // nah this makes the description tied to a particular reader offset would have diffent meanings.
    // on second thought, this may be the best option
    public class SimpleReader : IRecordReader
    {
        private readonly string _recordDelimiter;
        private string _line;
        private int _index;

        public int PartCount    //todo: rename
        {
            get { return 1; }
        }

        public SimpleReader(string recordDelimiter = "\r\n")
        {
            _recordDelimiter = recordDelimiter;
        }

        public bool ReadRecord(TextReader reader)
        {
            _line = reader.ReadLine();  //todo: implement record delimiter

            if (_line == null)
                return false;
            
            return true;
        }

        public string[] Current()
        {
            return new string[] { _line };
        }

        public string Fetch(int delimitedContext)
        {
            if (delimitedContext != 0)
                throw new ArgumentOutOfRangeException("delimitedContext");

            return _line;
        }

        public string Fetch(int delimitedContext, int index)
        {
            if (delimitedContext != 0)
                throw new ArgumentOutOfRangeException("delimitedContext");

            return _line.Substring(index);
        }

        public string Fetch(int delimitedContext, int index, int length)
        {
            if (delimitedContext != 0)
                throw new ArgumentOutOfRangeException("delimitedContext");

            //todo: length >= 0;

            return _line.Substring(index, length);
        }
    }
}
