using System.Linq.Expressions;

namespace VocaBuddy.Application.Interfaces;

public interface IGenericRepository<TEntity, TId> where TEntity : class
{
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, int? randomItemCount = default);

    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    void Remove(TEntity entity);
}
