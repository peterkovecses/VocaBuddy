using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class ForeignWordRepository : GenericRepository<ForeignWord, int>, IForeignWordRepository
{
	public ForeignWordRepository(VocaBuddyContext context) : base(context) { }

	public VocaBuddyContext VocaBuddyContext
		=> (Context as VocaBuddyContext)!;
}
