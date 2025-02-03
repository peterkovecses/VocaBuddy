namespace VocaBuddy.Application.Handlers;

public class GetLatestNativeWordsHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<GetLatestNativeWordsQuery, Result<List<CompactNativeWordDto>>>
{
    private readonly INativeWordRepository _nativeWords = unitOfWork.NativeWords;
    private readonly string _currentUserId = user.Id!;

    public async Task<Result<List<CompactNativeWordDto>>> Handle(GetLatestNativeWordsQuery request, CancellationToken cancellationToken)
    {
        var words = await _nativeWords.GetLatestAsync(request.WordCount, _currentUserId, cancellationToken);

        return Result.Success(words.RandomOrder().ToCompactNativeWordDto());
    }
}