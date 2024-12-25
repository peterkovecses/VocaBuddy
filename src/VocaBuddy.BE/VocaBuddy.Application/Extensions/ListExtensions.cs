namespace VocaBuddy.Application.Extensions;

public static class ListExtensions
{
    public static List<T> RandomOrder<T>(this List<T> source)
        => source.OrderBy(_ => Guid.NewGuid()).ToList();
}
