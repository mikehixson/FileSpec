using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class Package
    {
        public List<IMapping> Descriptions;
        public IRecordWriter Writer;
        public IRecordReader Reader;
        public Func<string, bool> Predicate;
        public Func<object> Create;

        public void Write(object record, TextWriter writer)
        {
            Writer.WriteStartRecord(writer);

            foreach (IMapping description in Descriptions)
                description.Write(record, Writer);

            Writer.WriteEndRecord(writer);
        }

        public bool Read(object record, TextReader reader)
        {
            bool read = Reader.ReadRecord(reader);

            if (read)
            {
                foreach (IMapping description in Descriptions)
                    description.Read(record, Reader);

                return true;
            }

            return false;
        }
    }
}
