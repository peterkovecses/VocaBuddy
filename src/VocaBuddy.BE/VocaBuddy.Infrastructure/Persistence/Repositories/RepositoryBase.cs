namespace VocaBuddy.Infrastructure.Persistence.Repositories;

internal abstract class RepositoryBase<TEntity, TEntityId>(DbContext context) where TEntity : EntityBase<TEntityId>
{
    protected readonly DbContext Context = context;

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public virtual void Remove(TEntity entity)
        => Context.Set<TEntity>().Remove(entity);
}
