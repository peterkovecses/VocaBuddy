using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Application.Interfaces;

public interface INativeWordRepository : IGenericRepository<NativeWord, int>
{
    Task<List<NativeWord>> GetRandomAsync(int count, string userId, CancellationToken cancellationToken);
    Task<List<NativeWord>> GetLatestAsync(int count, string userId, CancellationToken cancellationToken);
    Task<int> GetCountAsync(string userId, CancellationToken cancellationToken);
}
