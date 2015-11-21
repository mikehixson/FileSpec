using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSpec.Behavior;

namespace FileSpec
{
    public class PositionedField : IField
    {
        private readonly int _position;
        private readonly int _length;
        private readonly IMaximumLengthBehavior _maximumLength;
        private readonly IMinimumLengthBehavior _minimumLength;

        public int Position
        {
            get { return _position; }
        }

        public int Length
        {
            get { return _length; }
        }

        // length
        public PositionedField(int position, int length, FieldTruncate truncate = FieldTruncate.None, FieldPadding align = FieldPadding.Left)
        {
            _position = position;
            _length = length;
            _maximumLength = new MaximumLengthBehavior(length, truncate);
            _minimumLength = new MinimumLengthBehavior(length, align);
        }

        // max length
        public PositionedField(int position, int length, FieldTruncate truncate = FieldTruncate.None)
        {
            _position = position;
            _length = length;
            _maximumLength = new MaximumLengthBehavior(length, truncate);
        }

        // min length
        public PositionedField(int position, int length, FieldPadding align = FieldPadding.Left)
        {
            _position = position;
            _length = length;
            _minimumLength = new MinimumLengthBehavior(length, align);
        }

        public PositionedField(int position)
        {
            _position = position;
        }

        public void Write(IRecordWriter writer, string value)
        {
            if (_maximumLength != null)
                value = _maximumLength.Write(value);

            if (_minimumLength != null)
                value = _minimumLength.Write(value);

            writer.Write(value, _position, 0);
        }
        
        public string Read(List<string> values)
        {
            string value = values[_position];

            if (_minimumLength != null)
                value = _minimumLength.Read(value);

            return value;
        }
    }    
}
