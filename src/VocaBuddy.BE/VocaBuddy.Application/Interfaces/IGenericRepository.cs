namespace VocaBuddy.Application.Interfaces;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<List<TEntity>> GetAsync(CancellationToken token);

    Task<TEntity?> FindByIdAsync(TId id, CancellationToken token);

    Task AddAsync(TEntity entity, CancellationToken token);

    void Remove(TEntity entity);
}
