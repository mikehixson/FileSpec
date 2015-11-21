using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class DelimitedReader : IRecordReader
    {
        private readonly string _fieldDelimiter;
        private readonly string _recordDelimiter;
        private string[] _parts3;

        public int PartCount    //todo: rename
        {
            get { return _parts3.Length; }
        }

        public DelimitedReader(string fieldDelimiter = ",", string recordDelimiter = "\r\n")
        {
            _fieldDelimiter = fieldDelimiter;
            _recordDelimiter = recordDelimiter;
        }

        public bool Read(Reader reader)
        {
            return false;   // todo:
        }

        public bool ReadRecord(TextReader reader)
        {
            string line = reader.ReadLine();    //todo: implement record delimiter

            if (line == null)
                return false;

            _parts3 = line.Split(new string[] { _fieldDelimiter }, StringSplitOptions.None);   //todo: dont split, read char by char

            return true;
        }

        List<string> _parts = new List<string>();
        StringBuilder _buffer = new StringBuilder();
              

        public string[] Current()
        {
            return _parts.ToArray();
        }

        // only thing diffenrt between this and the fixed length one is how the delimited context is resolved.
        public string Fetch(int delimitedContext)
        {
            return _parts[delimitedContext];
        }

        public string Fetch(int delimitedContext, int index)
        {
            return _parts[delimitedContext].Substring(index);
        }

        public string Fetch(int delimitedContext, int index, int length)
        {
            return _parts[delimitedContext].Substring(index, length);
        }
    }    
}
