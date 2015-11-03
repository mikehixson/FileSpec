using System;

namespace FileSpec.Converter
{
	public partial class UnsignedNumberConverter
	{
		#region byte

		public string GetString(byte value)
		{
			string result;

			value = (byte)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		byte IConverter<byte>.GetValue(string text)
		{
			byte result = 0;

			if (Byte.TryParse(text, out result))
				result = (byte)(result / _scale);

			return result;
		}

		#endregion

		#region byte?

		public string GetString(byte? value)
		{
            return NullableHelper.GetString(value, this);
		}

		byte? IConverter<byte?>.GetValue(string text)
		{
            return NullableHelper.GetValue<byte>(text, this);
		}

		#endregion
		
		#region ushort

		public string GetString(ushort value)
		{
			string result;

			value = (ushort)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		ushort IConverter<ushort>.GetValue(string text)
		{
			ushort result = 0;

			if (UInt16.TryParse(text, out result))
				result = (ushort)(result / _scale);

			return result;
		}

		#endregion

		#region ushort?

		public string GetString(ushort? value)
		{
            return NullableHelper.GetString(value, this);
		}

		ushort? IConverter<ushort?>.GetValue(string text)
		{
            return NullableHelper.GetValue<ushort>(text, this);
		}

		#endregion
		
		#region uint

		public string GetString(uint value)
		{
			string result;

			value = (uint)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		uint IConverter<uint>.GetValue(string text)
		{
			uint result = 0;

			if (UInt32.TryParse(text, out result))
				result = (uint)(result / _scale);

			return result;
		}

		#endregion

		#region uint?

		public string GetString(uint? value)
		{
            return NullableHelper.GetString(value, this);
		}

		uint? IConverter<uint?>.GetValue(string text)
		{
            return NullableHelper.GetValue<uint>(text, this);
		}

		#endregion
		
		#region ulong

		public string GetString(ulong value)
		{
			string result;

			value = (ulong)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		ulong IConverter<ulong>.GetValue(string text)
		{
			ulong result = 0;

			if (UInt64.TryParse(text, out result))
				result = (ulong)(result / _scale);

			return result;
		}

		#endregion

		#region ulong?

		public string GetString(ulong? value)
		{
            return NullableHelper.GetString(value, this);
		}

		ulong? IConverter<ulong?>.GetValue(string text)
		{
            return NullableHelper.GetValue<ulong>(text, this);
		}

		#endregion
		
	}
}
