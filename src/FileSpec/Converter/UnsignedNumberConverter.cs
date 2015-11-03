using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Converter
{
    public partial class UnsignedNumberConverter :
        IConverter<byte>, IConverter<byte?>,
        IConverter<ushort>, IConverter<ushort?>,
        IConverter<uint>, IConverter<uint?>,
        IConverter<ulong>, IConverter<ulong?>
    {
        private readonly string _format;
        private readonly uint _scale;

        public UnsignedNumberConverter(string format = null, uint scale = 1)
        {
            _format = format;
            _scale = scale;
        }
    }
}
