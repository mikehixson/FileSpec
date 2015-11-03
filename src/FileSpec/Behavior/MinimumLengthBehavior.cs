using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec.Behavior
{
    /// <summary>
    /// Used to ensure that values have a minimum number of charaacters.
    /// </summary>
    public class MinimumLengthBehavior : IMinimumLengthBehavior
    {
        private readonly int _minLength;
        private readonly FieldPadding _padding;
        private readonly char _paddingChar;

        /// <summary>
        /// Initializes a new instance of the MinimumLengthBehavior class when given a minimum length and optinal alignment.
        /// </summary>
        /// <param name="minLength">The minimum number of characters that a value must have.</param>
        /// <param name="align">The side of the value that will be padded to meet the minimum required length.</param>
        public MinimumLengthBehavior(int minLength, FieldPadding padding = FieldPadding.Right)
        {
            _minLength = minLength;
            _padding = padding;
            _paddingChar = ' ';
        }

        /// <summary>
        /// Applies this behavior when writing a value to a file. If the value does not meet the minimum length
        /// requirements, it is padded so that it does.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns><paramref name="value"/> padded to the minimum required length.</returns>
        public string Write(string value)
        {
            if (value == null)
                return new String(_paddingChar, _minLength);

            if (_padding == FieldPadding.Right)
                return value.PadRight(_minLength);
            else
                return value.PadLeft(_minLength);
        }

        /// <summary>
        /// Applies this behavior when reading a value from a file. Removes padding from a value.
        /// </summary>
        /// <param name="value">The value read.</param>
        /// <returns><paramref name="value"/> with padding removed.</returns>
        public string Read(string value)
        {
            if (value == null)
                return null;

            if (_padding == FieldPadding.Right)
                return value.TrimEnd(_paddingChar);
            else
                return value.TrimStart(_paddingChar);
        }
    }
}
