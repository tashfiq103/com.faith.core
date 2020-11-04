
namespace com.faith.core
{
	using UnityEngine;
	using System;
	using System.Text;
	using System.Collections.Generic;
	using System.Security.Cryptography;

    public static class StringOperation
    {
		#region Configuretion

		private static string DecimalToHexNumeric(int value) {

			if (value == -1)
				return ".";
			else if (value == 10)
				return "A";
			else if (value == 11)
				return "B";
			else if (value == 12)
				return "C";
			else if (value == 13)
				return "D";
			else if (value == 14)
				return "E";
			else if (value == 15)
				return "F";
			else
				return value.ToString();
		}

		#endregion

		#region Public Callback

		public static string TruncateAllWhiteSpace(string t_ModifiableString)
		{

			string[] t_SplitByWhiteSpace = t_ModifiableString.Split(' ');
			string t_NewString = "";
			foreach (string t_SubString in t_SplitByWhiteSpace)
			{

				t_NewString += t_SubString;
			}

			return t_NewString;
		}

		public static bool IsSameString(string t_String1, string t_String2, bool t_TruncateWhiteSpace = false)
		{
			if (t_TruncateWhiteSpace)
			{

				t_String1 = TruncateAllWhiteSpace(t_String1);
				t_String2 = TruncateAllWhiteSpace(t_String2);
			}
			return String.Equals(t_String1, t_String2);
		}

		public static bool IsSameStringCaseInsensitive(string t_String1, string t_String2, bool t_TruncateWhiteSpace = false)
		{

			if (t_TruncateWhiteSpace)
			{
				t_String1 = TruncateAllWhiteSpace(t_String1);
				t_String2 = TruncateAllWhiteSpace(t_String2);
			}
			return String.Equals(t_String1, t_String2, StringComparison.OrdinalIgnoreCase);
		}

		public static string GetHexValue(float value, bool considerDecimalPoint = false)
		{

			if (value == 0)
				return "0";

			Stack<float> stackForDecimal = new Stack<float>();

			while (value > 0)
			{

				if (value < 16)
				{

					float floatValue = value % 16;
					stackForDecimal.Push((int)floatValue);

					if (!considerDecimalPoint)
					{
						value = 0;
						break;
					}
					else {

						//Decimal Fraction
						//----------------
						stackForDecimal.Push(-1);
						float decimalFraction = floatValue - ((int)floatValue);
						if (decimalFraction > 0)
							stackForDecimal.Push((int)(decimalFraction * 16));
						//----------------

						value = 0;
					}
				}
				else
				{
					
					stackForDecimal.Push(value % 16);
					value /= 16;
				}
			}

			string result = "";
			while (stackForDecimal.Count > 0)
			{
				int integerValue = (int)stackForDecimal.Pop();
				string hexValue = DecimalToHexNumeric(integerValue);
				result += hexValue;
			}

			string[] splitByDecimalPoint = result.Split('.');
			if (splitByDecimalPoint.Length > 1)
				return splitByDecimalPoint[1] + "." + splitByDecimalPoint[0];
			else
				return splitByDecimalPoint[0];
		}

		public static string GetHexColorFromRGBColor(Color color) {

			Vector3 _32BitColor = new Vector3(
				color.r * 255,
				color.g * 255,
				color.b * 255);

			return "#"
				+ (_32BitColor.x < 16 ? "0" : "") + GetHexValue(_32BitColor.x)
				+ (_32BitColor.y < 16 ? "0" : "") + GetHexValue(_32BitColor.y)
				+ (_32BitColor.z < 16 ? "0" : "") + GetHexValue(_32BitColor.z);
		}

		public static string GetSha256HashedKey(string value)
		{

			SHA256 sha256Hash = SHA256.Create();
			byte[] datas = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

			StringBuilder stringBuilder = new StringBuilder();

			foreach (byte data in datas)
			{

				stringBuilder.Append(data.ToString("x2"));
			}

			return stringBuilder.ToString();
		}

		#endregion




	}
}

