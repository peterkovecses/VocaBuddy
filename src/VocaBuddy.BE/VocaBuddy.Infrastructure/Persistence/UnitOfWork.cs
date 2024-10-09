namespace VocaBuddy.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly VocaBuddyContext _context;
    private readonly Lazy<INativeWordRepository> _nativeWords;
    private readonly Lazy<IForeignWordRepository> _foreignWords;

    public UnitOfWork(VocaBuddyContext context)
    {
        _context = context;
        _nativeWords = new Lazy<INativeWordRepository>(() => new NativeWordRepository(_context));
        _foreignWords = new Lazy<IForeignWordRepository>(() => new ForeignWordRepository(_context));
    }

    public INativeWordRepository NativeWords => _nativeWords.Value;
    public IForeignWordRepository ForeignWords => _foreignWords.Value;

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    private bool _disposed = false;

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
