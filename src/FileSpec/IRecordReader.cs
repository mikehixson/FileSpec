using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public interface IRecordReader
    {
        int PartCount { get; }   //todo: rename

        string[] Current(); //todo: rename
        bool ReadRecord(TextReader reader);

        string Fetch(int delimitedContext);
        string Fetch(int delimitedContext, int index);
        string Fetch(int delimitedContext, int index, int length);
    }
}
