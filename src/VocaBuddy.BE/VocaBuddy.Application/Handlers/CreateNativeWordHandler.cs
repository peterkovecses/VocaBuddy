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
        var nativeWord = _mapper.Map<NativeWord>(request.NativeWordDto);
        SetUserId(nativeWord);
        await SaveWord(nativeWord, cancellationToken);
        MapViewModel(request, nativeWord);

        return Result.Success(request.NativeWordDto);

        void SetUserId(NativeWord nativeWord)
            => nativeWord.UserId = _currentUserId;

        async Task SaveWord(NativeWord nativeWord, CancellationToken cancellationToken)
        {
            await _unitOfWork.NativeWords.AddAsync(nativeWord, cancellationToken);
            await _unitOfWork.CompleteAsync();
        }

        void MapViewModel(CreateNativeWordCommand request, NativeWord nativeWord)
        {
            MapWordId(request, nativeWord);
            MapTranslations(request, nativeWord);
        }

        static void MapWordId(CreateNativeWordCommand request, NativeWord nativeWord)
            => request.NativeWordDto.Id = nativeWord.Id;

        void MapTranslations(CreateNativeWordCommand request, NativeWord nativeWord)
            => request.NativeWordDto.Translations = _mapper.Map<List<ForeignWordDto>>(nativeWord.Translations);
    }
}
