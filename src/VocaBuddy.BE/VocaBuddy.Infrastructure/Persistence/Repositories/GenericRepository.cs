using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Infrastructure.Persistence.Extensions;

namespace VocaBuddy.Infrastructure.Persistence.Repositories;

public abstract class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
{
    protected readonly DbContext _context;

    public GenericRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token, int? randomItemCount = default)
    {
        return await _context
            .Set<TEntity>()
            .Where(predicate)
            .TakeRandom(randomItemCount)
            .ToListAsync(token);
    }

    public virtual async Task<TEntity?> FindByIdAsync(TId id, CancellationToken token)
        => await _context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: token);

    public async Task AddAsync(TEntity entity, CancellationToken token)
        => await _context.Set<TEntity>().AddAsync(entity, token);

    public void Remove(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);
}
