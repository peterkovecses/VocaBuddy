namespace VocaBuddy.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public INativeWordRepository NativeWords{ get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken);
}
