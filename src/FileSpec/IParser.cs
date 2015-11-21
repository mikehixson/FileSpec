using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public interface IParser
    {
        List<string> Current { get; }
        bool Parse();
    }
}
