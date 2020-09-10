
namespace com.faith.core
{

	using System;

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

		public static bool IsSameStringCI(string t_String1, string t_String2, bool t_TruncateWhiteSpace = false)
		{

			if (t_TruncateWhiteSpace)
			{
				t_String1 = TruncateAllWhiteSpace(t_String1);
				t_String2 = TruncateAllWhiteSpace(t_String2);
			}
			return String.Equals(t_String1, t_String2, StringComparison.OrdinalIgnoreCase);
		}
	}
}

