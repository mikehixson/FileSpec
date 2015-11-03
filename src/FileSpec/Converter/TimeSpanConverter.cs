using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public class TimeSpanConverter : IConverter<TimeSpan>, IConverter<TimeSpan?>
    {
        private string _format;

        public TimeSpanConverter(string format = null)
        {
            _format = format;
        }

        #region TimeSpan

        public string GetString(TimeSpan value)
        {
            return value.ToString(_format);
        }

        TimeSpan IConverter<TimeSpan>.GetValue(string text)
        {
            return TimeSpan.ParseExact(text, _format, null);
        }

        #endregion

        #region TimeSpan?

        public string GetString(TimeSpan? value)
        {
            return NullableHelper.GetString(value, this);
        }

        TimeSpan? IConverter<TimeSpan?>.GetValue(string text)
        {
            return NullableHelper.GetValue<TimeSpan>(text, this);
        }

        #endregion
    }
}
