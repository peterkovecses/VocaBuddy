using Microsoft.EntityFrameworkCore;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Infrastructure.Persistence.Extensions;

namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class NativeWordRepository : GenericRepository<NativeWord, int>, INativeWordRepository
{
	public NativeWordRepository(VocaBuddyContext context) : base(context)
	{
	}

	public VocaBuddyContext VocaBuddyContext
		=> _context as VocaBuddyContext;

    public async Task<List<NativeWord>> GetRandomAsync(int count, string userId, CancellationToken cancellationToken)
    {
        return await VocaBuddyContext.NativeWords
			.Where(word => word.UserId == userId)
            .TakeRandom(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<NativeWord>> GetLatestAsync(int count, string userId, CancellationToken cancellationToken)
    {
        return await VocaBuddyContext.NativeWords
            .Where(word => word.UserId == userId)
            .OrderByDescending(word => word.CreatedUtc)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetCountAsync(string userId, CancellationToken cancellationToken)
		=> VocaBuddyContext.NativeWords
			.Where(word => word.UserId == userId)
			.CountAsync(cancellationToken);
}
