namespace VocaBuddy.Infrastructure.Persistence.Repositories;

internal abstract class RepositoryBase<TEntity, TEntityId>(
    DbContext context) : IRepository<TEntity, TEntityId> where TEntity : EntityBase<TEntityId>
{
    protected DbSet<TEntity> DbSet => context.Set<TEntity>();

    public virtual async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken) 
        => await DbSet.FindAsync(new object[] { id }, cancellationToken);

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await DbSet.AddAsync(entity, cancellationToken);

    public virtual void Remove(TEntity entity)
        => DbSet.Remove(entity);
}