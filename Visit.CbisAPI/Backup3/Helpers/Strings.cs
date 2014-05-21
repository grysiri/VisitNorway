using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visit.CbisAPI.Helpers
{
	public static class Strings
	{
		public static string FirstUpper(this string str)
		{
			if (string.IsNullOrEmpty(str))
				return str;

			if (str.Length == 1)
				return str.ToUpper();

			return str.Substring(0, 1).ToUpper() + str.Substring(1);
		}

		public static string Implode<T>(this IEnumerable<T> enumerable, string delimiter)
		{
			if (enumerable == null || !enumerable.Any())
				return "";

			return enumerable.Select(e => e.ToString()).Aggregate((o, n) => o + delimiter + n);
		}
	}
}
