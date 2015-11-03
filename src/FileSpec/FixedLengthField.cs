using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSpec.Behavior;

namespace FileSpec
{
    // Fixed-length
    public class FixedLengthField : IField
    {
        private readonly int _index;
        private readonly int _length;
        private readonly IMaximumLengthBehavior _maximumLength;
        private readonly IMinimumLengthBehavior _minimumLength;

        public int Index
        {
            get { return _index; }
        }

        public int Length   //todo: do we need to expose these properties?
        {
            get { return _length; }
        }

        //todo: how about allowing min & max length to be differnt

        public FixedLengthField(int index, int length, FieldTruncate truncate = FieldTruncate.None, FieldAlign align = FieldAlign.Left)
        {
            _index = index;
            _length = length;
            _maximumLength = new MaximumLengthBehavior(length, truncate);
            _minimumLength = new MinimumLengthBehavior(length, align);
        }

        public FixedLengthField(int index)
        {
            _index = index;
            _length = -1;
        }

        public void Write(IRecordWriter writer, string value)
        {
            if (_maximumLength != null)
                value = _maximumLength.Write(value);

            if (_minimumLength != null)
                value = _minimumLength.Write(value);

            writer.Write(value, 0, _index);
        }

        public string Read(IRecordReader reader)
        {
            string value = reader.Fetch(0, _index, _length);

            if (_minimumLength != null)
                value = _minimumLength.Read(value);

            return value;
        }
    }
}
