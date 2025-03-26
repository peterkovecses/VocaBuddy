namespace VocaBuddy.Application.Interfaces;

public interface IRepository<TEntity, in TEntityId>
    where TEntity : EntityBase<TEntityId>
{
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Remove(TEntity entity);
}