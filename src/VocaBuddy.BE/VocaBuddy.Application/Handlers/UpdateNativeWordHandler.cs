using AutoMapper;
using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Application.Handlers;

public class UpdateNativeWordHandler : IRequestHandler<UpdateNativeWordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _currentUserId;
    private readonly IMapper _mapper;

    public UpdateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserId = user.Id!;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToUpdate
            = await _unitOfWork.NativeWords.FindByIdAsync(request.NativeWordDto.Id, cancellationToken)
                ?? throw new NotFoundException(request.NativeWordDto.Id);

        ThrowIfUserIdDoesNotMatch(nativeWordToUpdate);

        _mapper.Map(request.NativeWordDto, nativeWordToUpdate);
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }

    private void ThrowIfUserIdDoesNotMatch(NativeWord nativeWordToUpdate)
    {
        if (nativeWordToUpdate.UserId != _currentUserId)
        {
            throw new UserIdNotMatchException();
        }
    }
}
