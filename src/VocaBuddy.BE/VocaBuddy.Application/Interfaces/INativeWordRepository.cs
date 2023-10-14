using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Application.Interfaces;

public interface INativeWordRepository : IGenericRepository<NativeWord, int>
{
    Task<int> GetCountAsync(string userId, CancellationToken cancellationToken);
}
