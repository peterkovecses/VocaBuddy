namespace VocaBuddy.Application.Interfaces;

public interface ICommandRepository<in TEntity, in TEntityId>
    where TEntity : EntityBase<TEntityId>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Remove(TEntity entity);
}