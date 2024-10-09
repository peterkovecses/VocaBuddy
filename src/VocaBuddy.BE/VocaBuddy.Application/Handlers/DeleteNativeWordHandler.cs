namespace VocaBuddy.Application.Handlers;

public class DeleteNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<DeleteNativeWordCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _currentUserId = user.Id!;

    public async Task<Result> Handle(DeleteNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToDelete = await _unitOfWork.NativeWords.FindByIdAsync(request.WordId, cancellationToken);

        if (nativeWordToDelete is null)
        {
            return Result.Failure(ErrorInfoFactory.NotFound(request.WordId));
        }

        if(!ValidateUserId(nativeWordToDelete))
        {
            return Result.Failure(ErrorInfoFactory.UserIdNotMatch());
        }

        _unitOfWork.NativeWords.Remove(nativeWordToDelete);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    private bool ValidateUserId(NativeWord nativeWordToDelete)
        => nativeWordToDelete.UserId == _currentUserId;
}
