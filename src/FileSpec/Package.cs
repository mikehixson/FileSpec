using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    // No point for the package to take in a Reader/Parser. We will of had to parse before decising which package to use.
    // Also we keep the responsibility of the package to moving parsed values to properties, or 
    public class Package
    {
        public List<IMapping> Descriptions;
        public IRecordWriter Writer;        // these will need to go, thereader/write are file-level concerns
        public IRecordReader Reader;        // these will need to go
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
            return false;   //todo:
        }

        public void Read(object record, List<string> values)
        {
            foreach (IMapping description in Descriptions)
                description.Read(record, values);
        }
    }
}
