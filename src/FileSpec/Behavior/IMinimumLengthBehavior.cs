using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Behavior
{
    public interface IMinimumLengthBehavior
    {
        string Write(string value);
        string Read(string value);
    }
}
