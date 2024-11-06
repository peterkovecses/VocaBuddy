namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class ForeignWordRepository : GenericRepository<ForeignWord, int>, IForeignWordRepository
{
	public ForeignWordRepository(DbContext context) : base(context) { }

	public VocaBuddyContext VocaBuddyContext
		=> (Context as VocaBuddyContext)!;
}
