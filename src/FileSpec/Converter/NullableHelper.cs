using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public class NullableHelper
    {
        public static string GetString<T>(Nullable<T> value, IConverter<T> convert) where T : struct
        {
            if (value == null)
                return null;

            return convert.GetString(value.Value);
        }

        public static Nullable<T> GetValue<T>(string text, IConverter<T> convert) where T : struct
        {
            if (text == null)
                return null;

            return convert.GetValue(text);
        }
    }
}
