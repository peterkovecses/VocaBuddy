namespace VocaBuddy.Application.Interfaces;

public interface INativeWordRepository : IRepository<NativeWord, int>
{
    Task<List<NativeWord>> GetAsync(Expression<Func<NativeWord, bool>> predicate, CancellationToken cancellationToken);
    Task<List<NativeWord>> GetRandomAsync(int count, string userId, CancellationToken cancellationToken);
    Task<List<NativeWord>> GetLatestAsync(int count, string userId, CancellationToken cancellationToken);
    Task<List<NativeWord>> GetMistakenAsync(int count, string userId, CancellationToken cancellationToken);
    Task<int> GetCountAsync(string userId, CancellationToken cancellationToken);
    Task RecordMistakesAsync(string userId, IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken);
}
 