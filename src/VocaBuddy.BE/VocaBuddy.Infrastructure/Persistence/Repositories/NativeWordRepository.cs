namespace VocaBuddy.Infrastructure.Persistence.Repositories;

internal sealed class NativeWordRepository(DbContext context)
    : RepositoryBase<NativeWord, int>(context), INativeWordRepository
{
    public Task<List<NativeWord>> GetAsync(Expression<Func<NativeWord, bool>> predicate, CancellationToken cancellationToken)
        => DbSet
            .Where(predicate)
            .Include(word => word.Translations)
            .ToListAsync(cancellationToken);

    public Task<List<NativeWord>> GetRandomAsync(int count, string userId, CancellationToken cancellationToken)
        => DbSet
            .Where(word => word.UserId == userId)
            .TakeRandom(count)
            .Include(word => word.Translations)
            .ToListAsync(cancellationToken);

    public Task<List<NativeWord>> GetLatestAsync(int count, string userId, CancellationToken cancellationToken)
        => DbSet
            .Where(word => word.UserId == userId)
            .OrderByDescending(word => word.UpdatedUtc)
            .Take(count)
            .Include(word => word.Translations)
            .ToListAsync(cancellationToken);

    public Task<List<NativeWord>> GetMistakenAsync(int count, string userId, CancellationToken cancellationToken)
        => DbSet
            .Where(word => word.UserId == userId)
            .OrderByDescending(word => word.MistakeCount)
            .Take(count)
            .Include(word => word.Translations)
            .ToListAsync(cancellationToken);

    public override Task<NativeWord?> FindByIdAsync(int id, CancellationToken cancellationToken)
        => DbSet
            .Include(word => word.Translations)
            .FirstOrDefaultAsync(word => word.Id == id, cancellationToken);

    public Task<int> GetCountAsync(string userId, CancellationToken cancellationToken)
        => DbSet
            .Where(word => word.UserId == userId)
            .CountAsync(cancellationToken);

    public async Task RecordMistakesAsync(string userId, IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken) 
        => await DbSet
            .Where(word => word.UserId == userId && mistakenWordIds.Contains(word.Id))
            .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(word => word.MistakeCount, word => word.MistakeCount + 1),
                cancellationToken);
}
