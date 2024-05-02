namespace VocaBuddy.Application.Handlers;

public class GetRandomNativeWordsHandler : IRequestHandler<GetRandomNativeWordsQuery, Result<List<CompactNativeWordDto>>>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly string _currentUserId;
    private readonly IMapper _mapper;

    public GetRandomNativeWordsHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _currentUserId = user.Id!;
        _mapper = mapper;
    }

    public async Task<Result<List<CompactNativeWordDto>>> Handle(GetRandomNativeWordsQuery request, CancellationToken cancellationToken)
    {
        var words = await _nativeWords.GetRandomAsync(request.WordCount, _currentUserId, cancellationToken);

        return Result.Success(_mapper.Map<List<CompactNativeWordDto>>(words));
    }
}
