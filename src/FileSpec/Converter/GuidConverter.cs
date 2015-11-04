using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public class GuidConverter : IConverter<Guid>, IConverter<Guid?>
    {
        private readonly string _format;

        public GuidConverter(string format = null)
        {
            _format = format;
        }

        #region Guid

        public string GetString(Guid value)
        {
            return value.ToString(_format);
        }

        Guid IConverter<Guid>.GetValue(string value)
        {
            if (_format != null)
                return Guid.ParseExact(value, _format);

            return Guid.Parse(value);
        }

        #endregion

        #region Guid?

        public string GetString(Guid? value)
        {
            return NullableHelper.GetString(value, this);
        }

        Guid? IConverter<Guid?>.GetValue(string text)
        {
            return NullableHelper.GetValue<Guid>(text, this);
        }

        #endregion
    }
}
