using AutoMapper;
using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Handlers;

public class CreateNativeWordHandler : IRequestHandler<CreateNativeWordCommand, Result<NativeWordDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _currentUserId;
    private readonly IMapper _mapper;

    public CreateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserId = user.Id!;
        _mapper = mapper;
    }

    public async Task<Result<NativeWordDto>> Handle(CreateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWord = _mapper.Map<NativeWord>(request.NativeWorld);
        SetUserId();
        await SaveWordAsync();

        return Result.Success(_mapper.Map<NativeWordDto>(nativeWord));

        void SetUserId()
            => nativeWord.UserId = _currentUserId;

        async Task SaveWordAsync()
        {
            await _unitOfWork.NativeWords.AddAsync(nativeWord, cancellationToken);
            await _unitOfWork.CompleteAsync();
        }
    }
}
