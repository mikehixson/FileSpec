using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Behavior
{
    public class MinimumLengthBehavior : IMinimumLengthBehavior
    {
        private readonly int _minLength;
        private readonly FieldAlign _align;     // todo: maybe this is padding, then we could also indicate what the padding char is.

        public MinimumLengthBehavior(int minLength, FieldAlign align = FieldAlign.Left)
        {
            _minLength = minLength;
            _align = align;
        }

        public string Write(string value)
        {
            if (value == null)
                return new String(' ', _minLength);

            if (_align == FieldAlign.Left)
                return value.PadRight(_minLength);
            else
                return value.PadLeft(_minLength);
        }

        public string Read(string value)
        {
            if (_align == FieldAlign.Left)
                return value.TrimEnd(' ');
            else
                return value.TrimStart(' ');
        }
    }
}
