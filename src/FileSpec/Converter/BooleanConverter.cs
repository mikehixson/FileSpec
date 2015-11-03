using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    // todo: support for other than Y/N
    public class BooleanConverter : IConverter<bool>, IConverter<bool?>
    {
        #region bool

        public string GetString(bool value)
        {
            return value ? "Y" : "N";
        }

        public bool GetValue(string text)
        {
            bool result;

            if (String.Equals(text, "Y", StringComparison.OrdinalIgnoreCase))
                result = true;
            else if (String.Equals(text, "N", StringComparison.OrdinalIgnoreCase))
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
