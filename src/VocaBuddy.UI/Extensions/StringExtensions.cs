namespace VocaBuddy.UI.Extensions;

public static class StringExtensions
{
    public static string TrimQuotationMarks(this string source)
        => source.Trim('"');
}
