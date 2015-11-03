using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Behavior
{
    public class MaximumLengthBehavior : IMaximumLengthBehavior
    {
        private readonly int _maxLength;
        private readonly FieldTruncate _truncate;

        public MaximumLengthBehavior(int maxLength, FieldTruncate truncate = FieldTruncate.None)
        {
            // todo: maxlength must be greater than 0

            _maxLength = maxLength;
            _truncate = truncate;
        }

        public string Write(string value)
        {
            // Make sure the value will fit in the space allocated
            if (value != null && value.Length > _maxLength)
            {
                switch (_truncate)
                {
                    case FieldTruncate.Left:
                        value = value.Substring(value.Length - _maxLength);
                        break;

                    case FieldTruncate.Right:
                        value = value.Substring(0, _maxLength);
                        break;

                    //todo: how do we get the property name? catch exception outside?
                    case FieldTruncate.None:
                        throw new ApplicationException("Value is longer than space allocated.");
                }
            }

            return value;
        }
    }
}
