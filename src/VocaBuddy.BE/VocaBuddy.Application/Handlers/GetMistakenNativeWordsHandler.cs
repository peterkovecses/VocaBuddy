namespace VocaBuddy.Application.Handlers;

public class GetMistakenNativeWordsHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<GetMistakenNativeWordsQuery, Result<List<CompactNativeWordDto>>>
{
    private readonly INativeWordRepository _nativeWords = unitOfWork.NativeWords;
    private readonly string _currentUserId = user.Id!;
    
    public async Task<Result<List<CompactNativeWordDto>>> Handle(GetMistakenNativeWordsQuery request, CancellationToken cancellationToken)
    {
        var words = await _nativeWords.GetMistakenAsync(request.WordCount, _currentUserId, cancellationToken);

        return Result.Success(CompactNativeWordDtoMapper.FromDomainModel(words.RandomOrder()));
    }
}