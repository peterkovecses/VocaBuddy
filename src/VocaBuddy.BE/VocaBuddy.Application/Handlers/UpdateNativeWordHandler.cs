using AutoMapper;
using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Errors;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Handlers;

public class UpdateNativeWordHandler : IRequestHandler<UpdateNativeWordCommand, Result>
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

    public async Task<Result> Handle(UpdateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToUpdate
            = await _unitOfWork.NativeWords.FindByIdAsync(request.NativeWordDto.Id, cancellationToken);

        if (nativeWordToUpdate is null)
        {
            return Result.Failure(ErrorInfoFactory.NotFound(request.NativeWordDto.Id));
        }

        if (!ValidateUserId(nativeWordToUpdate))
        {
            return Result.Failure(ErrorInfoFactory.UserIdNotMatch());
        }

        _mapper.Map(request.NativeWordDto, nativeWordToUpdate);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    private bool ValidateUserId(NativeWord nativeWordToDelete)
        => nativeWordToDelete.UserId == _currentUserId;
}
