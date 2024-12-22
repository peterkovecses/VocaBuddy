namespace VocaBuddy.Application.Handlers;

public class UpdateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper) : IRequestHandler<UpdateNativeWordCommand, Result>
{
    private readonly string _currentUserId = user.Id!;

    public async Task<Result> Handle(UpdateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToUpdate
            = await unitOfWork.NativeWords.FindByIdAsync(request.NativeWord.Id, cancellationToken);

        if (nativeWordToUpdate is null)
        {
            return Result.Failure(ErrorInfoFactory.NotFound(request.NativeWord.Id));
        }

        if (!ValidateUserId(nativeWordToUpdate))
        {
            return Result.Failure(ErrorInfoFactory.UserIdNotMatch());
        }

        await UpdateWordAsync();

        return Result.Success();

        bool ValidateUserId(NativeWord nativeWordToDelete)
            => nativeWordToDelete.UserId == _currentUserId;

        async Task UpdateWordAsync()
        {
            mapper.Map(request.NativeWord, nativeWordToUpdate);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
