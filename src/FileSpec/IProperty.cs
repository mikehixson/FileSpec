using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public interface IProperty
    {
        string Get(object record);
        void Set(object record, string value);
    }
}
