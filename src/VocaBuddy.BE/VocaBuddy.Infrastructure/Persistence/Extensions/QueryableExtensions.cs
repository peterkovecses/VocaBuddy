namespace VocaBuddy.Infrastructure.Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> TakeRandom<T>(this IQueryable<T> source, int count)
        => source
            .RandomOrder()
            .Take(count);

    public static IQueryable<T> RandomOrder<T>(this IQueryable<T> source)
        => source.OrderBy(_ => Guid.NewGuid());
}
