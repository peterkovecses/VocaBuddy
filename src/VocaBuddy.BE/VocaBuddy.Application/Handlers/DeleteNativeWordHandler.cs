using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;

namespace VocaBuddy.Application.Handlers;

public class DeleteNativeWordHandler : IRequestHandler<DeleteNativeWordCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNativeWordHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToDelete = await _unitOfWork.NativeWords.FindByIdAsync(request.Id, cancellationToken);

        if (nativeWordToDelete is null)
        {
            throw new NotFoundException(request.Id);
        }

        _unitOfWork.NativeWords.Remove(nativeWordToDelete);
        _unitOfWork.CompleteAsync();
    }
}
