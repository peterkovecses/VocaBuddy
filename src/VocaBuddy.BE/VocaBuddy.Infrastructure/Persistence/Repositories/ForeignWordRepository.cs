namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public class ForeignWordRepository(DbContext context)
	: GenericRepository<ForeignWord, int>(context), IForeignWordRepository
{
	private VocaBuddyContext VocaBuddyContext
		=> (Context as VocaBuddyContext)!;
}
