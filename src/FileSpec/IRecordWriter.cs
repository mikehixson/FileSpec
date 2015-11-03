using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public interface IRecordWriter
    {
        void Write(string value, int delimitedContext);
        void Write(string value, int delimitedContext, int index);
        void WriteStartRecord(TextWriter writer);
        void WriteEndRecord(TextWriter writer);
        
    }
}
