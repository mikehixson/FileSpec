using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public class BooleanConverter : IConverter<bool>, IConverter<bool?>
    {
        private readonly string _trueValue;
        private readonly string _falseValue;

        public BooleanConverter(string trueValue = "True", string falseValue = "False")
        {
            _trueValue = trueValue;
            _falseValue = falseValue;
        }

        #region bool

        public string GetString(bool value)
        {
            return value ? _trueValue : _falseValue;
        }

        public bool GetValue(string text)
        {
            bool result;

            if (String.Equals(text, _trueValue, StringComparison.OrdinalIgnoreCase))
                result = true;
            else if (String.Equals(text, _falseValue, StringComparison.OrdinalIgnoreCase))
                result = false;
            else
                throw new ApplicationException("Unexpected value for boolean");

            return result;
        }

        #endregion

        #region bool?

        public string GetString(bool? value)
        {
            return NullableHelper.GetString(value, this);
        }

        bool? IConverter<bool?>.GetValue(string text)
        {
            return NullableHelper.GetValue<bool>(text, this);
        }

        #endregion
    }
}
