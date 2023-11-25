namespace VocaBuddy.Shared.Extensions;

public static class EnumerableExtensions
{
    public static bool AllUnique<T>(this IEnumerable<T> items)
        => items
            .GroupBy(item => item)
            .All(group => group.Count() == 1);
}
