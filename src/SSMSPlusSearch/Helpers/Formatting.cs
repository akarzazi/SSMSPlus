namespace SSMSPlusSearch.Helpers
{
    public static class Formatting
    {
        public static string FormatDatatype(string datatype, int? precision, int? scale)
        {
            if (precision == null || precision == 0)
                return datatype;

            if (precision == -1)
                return $"{datatype}(max)";

            if (scale == null || scale == 0)
                return $"{datatype}({ precision })";

            return $"{datatype}({ precision },{scale})";
        }
    }
}
