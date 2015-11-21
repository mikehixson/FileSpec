using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    // Experimiental
    public class NamedField : IField
    {
        private readonly int _position;
        private readonly string _name;

        public int Position
        {
            get { return _position; }
        }

        public string Name
        {
            get { return _name; }
        }

        public NamedField(int position, string name)
        {
            _position = position;
            _name = name;
        }

        public void Write(IRecordWriter writer, string value)
        {
            value = String.Format("{0}={1}", _name, value); // is this a behavior?  It could tell us how to write the value and how to match & parse on read

            writer.Write(value, _position);
        }

        public string Read(IRecordReader reader)
        {
            // assume that the value for this field can be at any position

            for (int i = 0; i < reader.PartCount; i++)       // maybe we expose parts as IEnumerable?. We need to know i and the value
            {
                string value = reader.Fetch(i);

                if (value.StartsWith(_name))
                    return value.Substring(value.IndexOf('=') + 1);
            }

            return null;
        }

        public string Read(List<string> values)
        {
            // assume that the value for this field can be at any position

            for (int i = 0; i < values.Count; i++)  //todo: use foreach?
            {
                string value = values[i];

                if (value.StartsWith(_name))
                    return value.Substring(value.IndexOf('=') + 1);
            }

            return null;
        }
    }
}
