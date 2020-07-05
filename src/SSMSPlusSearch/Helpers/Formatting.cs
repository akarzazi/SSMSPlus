using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.Helpers
{
    public static class Formatting
    {
        public static string FormatDatatype(string datatype, string precision, string scale)
        {
            if (precision == "-1")
            {
                datatype += "(max)";
            }
            else if(string.IsNullOrWhiteSpace(scale) == false)
            {
                datatype += $"({ precision },{scale})";
            }
            else
            {
                datatype += $"({ precision })";
            }
            return datatype;
        }
    }
}
