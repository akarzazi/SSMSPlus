using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Helpers
{
    public static class Formatting
    {
        public static string FormatDatatype(string datatype, string precision)
        {
            if (precision == "-1")
            {
                datatype += "(max)";
            }
            else
            {
                datatype += "(" + precision + ")";
            }
            return datatype;
        }
    }
}
