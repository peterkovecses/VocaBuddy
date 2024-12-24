﻿namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class NativeWordRepository(DbContext context)
    : GenericRepository<NativeWord, int>(context), INativeWordRepository
{
    private VocaBuddyContext VocaBuddyContext
        => (Context as VocaBuddyContext)!;

    public override Task<List<NativeWord>> GetAsync(Expression<Func<NativeWord, bool>> predicate, CancellationToken cancellationToken)
        => VocaBuddyContext.NativeWords
            .Include(word => word.Translations)
            .Where(predicate)
            .ToListAsync(cancellationToken);

    public async Task<List<NativeWord>> GetRandomAsync(int count, string userId, CancellationToken cancellationToken)
        => await VocaBuddyContext.NativeWords
            .Include(word => word.Translations)
            .Where(word => word.UserId == userId)
            .TakeRandom(count)
            .ToListAsync(cancellationToken);

    public async Task<List<NativeWord>> GetLatestAsync(int count, string userId, CancellationToken cancellationToken)
        => (await VocaBuddyContext.NativeWords
                .Include(word => word.Translations)
                .Where(word => word.UserId == userId)
                .OrderByDescending(word => word.UpdatedUtc)
                .Take(count)
                .ToListAsync(cancellationToken))
            .RandomOrder();
    
    public async Task<List<NativeWord>> GetMistakenAsync(int count, string userId, CancellationToken cancellationToken)
        => (await VocaBuddyContext.NativeWords
                .Include(word => word.Translations)
                .Where(word => word.UserId == userId)
                .OrderByDescending(word => word.MistakeCount)
                .Take(count)
                .ToListAsync(cancellationToken))
            .RandomOrder();

    public override Task<NativeWord?> FindByIdAsync(int id, CancellationToken cancellationToken)
        => VocaBuddyContext.NativeWords
            .Include(word => word.Translations)
            .FirstOrDefaultAsync(word => word.Id == id, cancellationToken);

    public Task<int> GetCountAsync(string userId, CancellationToken cancellationToken)
        => VocaBuddyContext.NativeWords
            .Where(word => word.UserId == userId)
            .CountAsync(cancellationToken);

    public async Task RecordMistakesAsync(string userId, IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken) 
        => await VocaBuddyContext.NativeWords
            .Where(word => word.UserId == userId && mistakenWordIds.Contains(word.Id))
            .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(word => word.MistakeCount, word => word.MistakeCount + 1),
                cancellationToken);
}
