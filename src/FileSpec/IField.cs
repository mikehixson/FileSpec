using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public interface IField
    {
        void Write(IRecordWriter writer, string value);
        string Read(IRecordReader reader);
    }
}
