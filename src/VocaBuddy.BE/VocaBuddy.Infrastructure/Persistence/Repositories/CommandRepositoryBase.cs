﻿namespace VocaBuddy.Infrastructure.Persistence.Repositories;

internal abstract class CommandRepositoryBase<TEntity, TEntityId>(
    DbContext context) : ICommandRepository<TEntity, TEntityId> where TEntity : EntityBase<TEntityId>
{
    protected DbSet<TEntity> DbSet => context.Set<TEntity>();

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await DbSet.AddAsync(entity, cancellationToken);

    public virtual void Remove(TEntity entity)
        => DbSet.Remove(entity);
}