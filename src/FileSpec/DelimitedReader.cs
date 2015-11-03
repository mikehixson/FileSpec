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
        private readonly TextReader _reader;
        private readonly string _fieldDelimiter;
        private readonly string _recordDelimiter;
        private string[] _parts;

        public int PartCount    //todo: rename
        {
            get { return _parts.Length; }
        }

        public DelimitedReader(string fieldDelimiter = ",", string recordDelimiter = "\r\n")
        {
            _fieldDelimiter = fieldDelimiter;
            _recordDelimiter = recordDelimiter;
        }
        
        public bool ReadRecord(TextReader reader)
        {
            string line = reader.ReadLine();    //todo: implement record delimiter

            if (line == null)
                return false;

            _parts = line.Split(new string[] { _fieldDelimiter }, StringSplitOptions.None);   //todo: dont split, read char by char

            return true;
        }

        public string[] Current()
        {
            return _parts;
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
