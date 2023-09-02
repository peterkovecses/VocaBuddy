using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;

namespace VocaBuddy.Application.Handlers;

public class DeleteNativeWordHandler : IRequestHandler<DeleteNativeWordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNativeWordHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToDelete = await _unitOfWork.NativeWords.FindByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(request.Id);

        if (nativeWordToDelete.UserId != request.UserId)
        {
            throw new UserIdNotMatchException();
        }

        _unitOfWork.NativeWords.Remove(nativeWordToDelete);
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
