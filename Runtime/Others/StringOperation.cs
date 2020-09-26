
namespace com.faith.core
{

	using System;
	using System.Text;
	using System.Security.Cryptography;

    public static class StringOperation
    {
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

		public static string GetSha256HashedKey(string value) {

			SHA256 sha256Hash = SHA256.Create();
			byte[] datas = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

			StringBuilder stringBuilder = new StringBuilder();

			foreach (byte data in datas) {

				stringBuilder.Append(data.ToString("x2"));
			}

			return stringBuilder.ToString();
		}
	}
}

