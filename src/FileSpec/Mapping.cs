using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class Mapping : IMapping
    {
        public IProperty Property { get; set; }
        public IField Field { get; set; }

        public void Write(object record, IRecordWriter writer)
        {
            string value = Property.Get(record);

            Field.Write(writer, value);
        }

        public void Read(object record, IRecordReader reader)
        {
            string value = Field.Read(reader);

            Property.Set(record, value);
        }
        
        public void Read(object record, List<string> values)
        {
            string value = Field.Read(values);

            Property.Set(record, value);
        }
    }
}
