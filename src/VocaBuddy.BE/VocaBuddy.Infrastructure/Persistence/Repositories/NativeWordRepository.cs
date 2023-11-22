using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Infrastructure.Persistence.Extensions;

namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class NativeWordRepository : GenericRepository<NativeWord, int>, INativeWordRepository
{
    public NativeWordRepository(VocaBuddyContext context) : base(context) { }

    public VocaBuddyContext VocaBuddyContext
        => Context as VocaBuddyContext;

    public override Task<List<NativeWord>> GetAsync(Expression<Func<NativeWord, bool>> predicate, CancellationToken cancellationToken)
        => VocaBuddyContext.NativeWords
            .Include(word => word.Translations)
            .Where(predicate)
            .ToListAsync(cancellationToken);

    public override Task<NativeWord?> FindByIdAsync(int id, CancellationToken cancellationToken)
        => VocaBuddyContext.NativeWords
            .Include(word => word.Translations)
            .Where(word => word.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

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

    public Task<int> GetCountAsync(string userId, CancellationToken cancellationToken)
        => VocaBuddyContext.NativeWords
            .Where(word => word.UserId == userId)
            .CountAsync(cancellationToken);
}
