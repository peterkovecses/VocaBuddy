namespace VocaBuddy.Application.Handlers;

public class CreateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<CreateNativeWordCommand, Result<NativeWordDto>>
{
    private readonly string _currentUserId = user.Id!;

    public async Task<Result<NativeWordDto>> Handle(CreateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWord = request.NativeWord.ToDomainModel();
        SetUserId();
        await SaveWordAsync();

        return Result.Success(nativeWord.ToNativeWordDto());

        void SetUserId()
            => nativeWord.UserId = _currentUserId;

        async Task SaveWordAsync()
        {
            await unitOfWork.NativeWords.AddAsync(nativeWord, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
