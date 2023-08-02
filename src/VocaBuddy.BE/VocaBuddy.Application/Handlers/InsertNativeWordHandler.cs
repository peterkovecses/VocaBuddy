using AutoMapper;
using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Handlers;

public class InsertNativeWordHandler : IRequestHandler<InsertNativeWordCommand, NativeWordDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InsertNativeWordHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NativeWordDto> Handle(InsertNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWord = _mapper.Map<NativeWord>(request.NativeWordDto);
        await _unitOfWork.NativeWords.AddAsync(nativeWord, cancellationToken);
        await _unitOfWork.CompleteAsync();
        request.NativeWordDto.Id = nativeWord.Id;
        request.NativeWordDto.Translations = _mapper.Map<List<ForeignWordDto>>(nativeWord.Translations);

        return request.NativeWordDto;
    }
}
