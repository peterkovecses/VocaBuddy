using Microsoft.EntityFrameworkCore;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class NativeWordRepository : GenericRepository<NativeWord, int>, INativeWordRepository
{
	public NativeWordRepository(VocaBuddyContext context) : base(context)
	{
	}

	public VocaBuddyContext VocaBuddyContext
		=> _context as VocaBuddyContext;

    public Task<int> GetCountAsync(string userId, CancellationToken cancellationToken)
		=> VocaBuddyContext.NativeWords
			.Where(word => word.UserId == userId)
			.CountAsync(cancellationToken);
}
