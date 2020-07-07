namespace SSMSPlusSearch.Helpers
{
    public static class Formatting
    {
        public static string FormatDatatype(string datatype, long? precision, long? scale)
        {
            if (precision == null || precision == 0)
                return datatype;

            if (precision == -1)
                return $"{datatype}(max)";

            if (scale == null)
                return $"{datatype}({ precision })";

            return $"{datatype}({ precision },{scale})";
        }
    }
}
