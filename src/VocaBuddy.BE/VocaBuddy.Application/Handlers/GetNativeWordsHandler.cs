namespace VocaBuddy.Application.Handlers;

public class GetNativeWordsHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<GetNativeWordsQuery, Result<List<NativeWordDto>>>
{
    private readonly INativeWordRepository _nativeWords = unitOfWork.NativeWords;
    private readonly string _currentUserId = user.Id!;

    public async Task<Result<List<NativeWordDto>>> Handle(
        GetNativeWordsQuery request,
        CancellationToken cancellationToken)
    {
        Expression<Func<NativeWord, bool>> predicate = word => word.UserId == _currentUserId;
        var words = await _nativeWords.GetAsync(predicate, cancellationToken);

        return Result.Success(words.ToNativeWordDto());
    }
}
