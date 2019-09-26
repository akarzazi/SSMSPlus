using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsText(this string str, string substring, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
        {
            return str.IndexOf(substring, comp) >= 0;
        }

        public static string Truncate(this string str, int lenght)
        {
            return str.Substring(0, Math.Min(str.Length, lenght));
        }

        public static string RemoveLineReturns(this string str)
        {
            return str.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        }
    }
}
