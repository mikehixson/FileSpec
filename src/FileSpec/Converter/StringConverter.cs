using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    //todo: how about UpperCase? Otherthings would like uppercase as well (certain date formats)
    public class StringConverter : IConverter<string>
    {
        public string GetString(string value)
        {
            return value;
        }

        public string GetValue(string value)
        {
            return value;
        }
    }
}
