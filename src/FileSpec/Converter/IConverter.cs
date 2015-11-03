using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public interface IConverter<T>
    {
        string GetString(T value);
        T GetValue(string text);
    }
}
