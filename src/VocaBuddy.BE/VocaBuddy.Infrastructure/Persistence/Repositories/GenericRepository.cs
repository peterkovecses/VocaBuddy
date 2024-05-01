using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public abstract class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
{
    protected readonly DbContext Context;

    protected GenericRepository(DbContext context)
    {
        Context = context;
    }

    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Context
            .Set<TEntity>()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken)
        => await Context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Remove(TEntity entity)
        => Context.Set<TEntity>().Remove(entity);
}
