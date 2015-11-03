using System;

namespace FileSpec.Converter
{
	public partial class NumberConverter
	{
		#region sbyte

		public string GetString(sbyte value)
		{
			string result;

			value = (sbyte)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		sbyte IConverter<sbyte>.GetValue(string text)
		{
			sbyte result = 0;

			if (SByte.TryParse(text, out result))
				result = (sbyte)(result / _scale);

			return result;
		}

		#endregion

		#region sbyte?

		public string GetString(sbyte? value)
		{
            return NullableHelper.GetString(value, this);
		}

		sbyte? IConverter<sbyte?>.GetValue(string text)
		{
            return NullableHelper.GetValue<sbyte>(text, this);
		}

		#endregion
		
		#region short

		public string GetString(short value)
		{
			string result;

			value = (short)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		short IConverter<short>.GetValue(string text)
		{
			short result = 0;

			if (Int16.TryParse(text, out result))
				result = (short)(result / _scale);

			return result;
		}

		#endregion

		#region short?

		public string GetString(short? value)
		{
            return NullableHelper.GetString(value, this);
		}

		short? IConverter<short?>.GetValue(string text)
		{
            return NullableHelper.GetValue<short>(text, this);
		}

		#endregion
		
		#region int

		public string GetString(int value)
		{
			string result;

			value = (int)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		int IConverter<int>.GetValue(string text)
		{
			int result = 0;

			if (Int32.TryParse(text, out result))
				result = (int)(result / _scale);

			return result;
		}

		#endregion

		#region int?

		public string GetString(int? value)
		{
            return NullableHelper.GetString(value, this);
		}

		int? IConverter<int?>.GetValue(string text)
		{
            return NullableHelper.GetValue<int>(text, this);
		}

		#endregion
		
		#region long

		public string GetString(long value)
		{
			string result;

			value = (long)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		long IConverter<long>.GetValue(string text)
		{
			long result = 0;

			if (Int64.TryParse(text, out result))
				result = (long)(result / _scale);

			return result;
		}

		#endregion

		#region long?

		public string GetString(long? value)
		{
            return NullableHelper.GetString(value, this);
		}

		long? IConverter<long?>.GetValue(string text)
		{
            return NullableHelper.GetValue<long>(text, this);
		}

		#endregion
		
		#region decimal

		public string GetString(decimal value)
		{
			string result;

			value = (decimal)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		decimal IConverter<decimal>.GetValue(string text)
		{
			decimal result = 0;

			if (Decimal.TryParse(text, out result))
				result = (decimal)(result / _scale);

			return result;
		}

		#endregion

		#region decimal?

		public string GetString(decimal? value)
		{
            return NullableHelper.GetString(value, this);
		}

		decimal? IConverter<decimal?>.GetValue(string text)
		{
            return NullableHelper.GetValue<decimal>(text, this);
		}

		#endregion
		
		#region float

		public string GetString(float value)
		{
			string result;

			value = (float)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		float IConverter<float>.GetValue(string text)
		{
			float result = 0;

			if (Single.TryParse(text, out result))
				result = (float)(result / _scale);

			return result;
		}

		#endregion

		#region float?

		public string GetString(float? value)
		{
            return NullableHelper.GetString(value, this);
		}

		float? IConverter<float?>.GetValue(string text)
		{
            return NullableHelper.GetValue<float>(text, this);
		}

		#endregion
		
		#region double

		public string GetString(double value)
		{
			string result;

			value = (double)(value * _scale);
			result = value.ToString(_format);

			return result;
		}

		double IConverter<double>.GetValue(string text)
		{
			double result = 0;

			if (Double.TryParse(text, out result))
				result = (double)(result / _scale);

			return result;
		}

		#endregion

		#region double?

		public string GetString(double? value)
		{
            return NullableHelper.GetString(value, this);
		}

		double? IConverter<double?>.GetValue(string text)
		{
            return NullableHelper.GetValue<double>(text, this);
		}

		#endregion
		
	}
}
