using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public partial class NumberConverter :
        IConverter<sbyte>, IConverter<sbyte?>,
        IConverter<short>, IConverter<short?>,
        IConverter<int>, IConverter<int?>,
        IConverter<long>, IConverter<long?>,
        IConverter<decimal>, IConverter<decimal?>,
        IConverter<float>, IConverter<float?>,
        IConverter<double>, IConverter<double?>
    {
        private readonly string _format;
        private readonly int _scale;

        public NumberConverter(string format = null, int scale = 1)
        {
            _format = format;
            _scale = scale;
        }
    }
}
