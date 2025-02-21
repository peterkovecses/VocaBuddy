namespace VocaBuddy.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly VocaBuddyContext _context;
    private readonly Lazy<INativeWordRepository> _nativeWords;

    public UnitOfWork(VocaBuddyContext context)
    {
        _context = context;
        _nativeWords = new Lazy<INativeWordRepository>(() => new NativeWordRepository(_context));
    }

    public INativeWordRepository NativeWords => _nativeWords.Value;

    public async Task<int> CompleteAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);

    private bool _disposed;

    ~UnitOfWork() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }
}
