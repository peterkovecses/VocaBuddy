namespace VocaBuddy.Infrastructure.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> TakeRandom<T>(this IQueryable<T> source, int? count)
    {
        if (count is null)
        {
            return source;
        }

        return source
            .OrderBy(item => Guid.NewGuid())
            .Take(count.Value);
    }
}
