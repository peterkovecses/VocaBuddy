using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Application.Handlers;

public class DeleteNativeWordHandler : IRequestHandler<DeleteNativeWordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _currentUserId;

    public DeleteNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user)
    {
        _unitOfWork = unitOfWork;
        _currentUserId = user.Id!;
    }

    public async Task<Unit> Handle(DeleteNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToDelete = await _unitOfWork.NativeWords.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(request.Id);

        ThrowIfUserIdDoesNotMatch(nativeWordToDelete);

        _unitOfWork.NativeWords.Remove(nativeWordToDelete);
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }

    private void ThrowIfUserIdDoesNotMatch(NativeWord nativeWordToDelete)
    {
        if (nativeWordToDelete.UserId != _currentUserId)
        {
            throw new UserIdNotMatchException();
        }
    }
}
