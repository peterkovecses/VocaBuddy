namespace VocaBuddy.Application.Extensions;

public static class IEnumerableExtensions
{
    public static bool AllUnique<T>(this IEnumerable<T> items)
        => items
            .GroupBy(item => item)
            .All(group => group.Count() == 1);
}
