using AutoMapper;
using MediatR;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Handlers;

public class GetLatestNativeWordsHandler : IRequestHandler<GetLatestNativeWordsQuery, Result<List<CompactNativeWordDto>>>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly string _currentUserId;
    private readonly IMapper _mapper;

    public GetLatestNativeWordsHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _currentUserId = user.Id!;
        _mapper = mapper;
    }

    public async Task<Result<List<CompactNativeWordDto>>> Handle(GetLatestNativeWordsQuery request, CancellationToken cancellationToken)
    {
        var words = await _nativeWords.GetLatestAsync(request.WordCount, _currentUserId, cancellationToken);

        return Result.Success(_mapper.Map<List<CompactNativeWordDto>>(words));
    }
}
