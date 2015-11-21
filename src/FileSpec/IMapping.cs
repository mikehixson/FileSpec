using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public interface IMapping
    {
        //IProperty Property { get; }
        //IField Field { get; set; }

        void Write(object record, IRecordWriter writer);
        void Read(object record, List<string> values);
    }
}
