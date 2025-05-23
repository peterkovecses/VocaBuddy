﻿namespace VocaBuddy.Application.Features.NativeWords.Commands.Delete;

public class DeleteNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<DeleteNativeWordCommand, Result>
{
    private readonly string _currentUserId = user.Id!;

    public async Task<Result> Handle(DeleteNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToDelete = await unitOfWork.NativeWords.FindByIdAsync(request.WordId, cancellationToken);

        if (nativeWordToDelete is null)
        {
            return Result.Failure(ErrorInfoFactory.NotFound(request.WordId));
        }

        if(!ValidateUserId(nativeWordToDelete))
        {
            return Result.Failure(ErrorInfoFactory.UserIdNotMatch());
        }

        unitOfWork.NativeWords.Remove(nativeWordToDelete);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    private bool ValidateUserId(NativeWord nativeWordToDelete)
        => nativeWordToDelete.UserId == _currentUserId;
}
