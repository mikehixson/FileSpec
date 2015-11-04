using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public class DateTimeConverter : IConverter<DateTime>, IConverter<DateTime?>
    {
        private readonly string _format;

        public DateTimeConverter(string format = null)
        {
            _format = format;
        }

        #region DateTime

        public string GetString(DateTime value)
        {
            return value.ToString(_format);
        }

        DateTime IConverter<DateTime>.GetValue(string value)
        {
            if (_format != null)
                return DateTime.ParseExact(value, _format, null);

            return DateTime.Parse(value);
        }

        #endregion

        #region DateTime?

        public string GetString(DateTime? value)
        {
            return NullableHelper.GetString(value, this);
        }

        DateTime? IConverter<DateTime?>.GetValue(string text)
        {
            return NullableHelper.GetValue<DateTime>(text, this);
        }

        #endregion
    }
}
