namespace VocaBuddy.Application.Handlers;

public class CreateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper) : IRequestHandler<CreateNativeWordCommand, Result<NativeWordDto>>
{
    private readonly string _currentUserId = user.Id!;

    public async Task<Result<NativeWordDto>> Handle(CreateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWord = mapper.Map<NativeWord>(request.NativeWord);
        SetUserId();
        await SaveWordAsync();

        return Result.Success(mapper.Map<NativeWordDto>(nativeWord));

        void SetUserId()
            => nativeWord.UserId = _currentUserId;

        async Task SaveWordAsync()
        {
            await unitOfWork.NativeWords.AddAsync(nativeWord, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
