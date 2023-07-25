using AutoMapper;
using MediatR;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Handlers;

public class GetNativeWordsHandler : IRequestHandler<GetNativeWordsQuery, List<NativeWordDto>>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly IMapper _mapper;

    public GetNativeWordsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _mapper = mapper;
    }

    public async Task<List<NativeWordDto>> Handle(
        GetNativeWordsQuery request,
        CancellationToken cancellationToken)
    {
        var nativeWords = await _nativeWords.GetAsync(cancellationToken);

        return _mapper.Map<List<NativeWordDto>>(nativeWords);
    }
}
