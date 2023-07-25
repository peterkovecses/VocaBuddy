namespace VocaBuddy.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public INativeWordRepository NativeWords{ get; }
        public IForeignWordRepository ForeignWords { get; }

        Task<int> CompleteAsync();
    }
}
