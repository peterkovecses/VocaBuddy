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
            = await _unitOfWork.NativeWords.FindByIdAsync(request.NativeWord.Id, cancellationToken);

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
            _mapper.Map(request.NativeWord, nativeWordToUpdate);
            await _unitOfWork.CompleteAsync();
        }
    }
}
