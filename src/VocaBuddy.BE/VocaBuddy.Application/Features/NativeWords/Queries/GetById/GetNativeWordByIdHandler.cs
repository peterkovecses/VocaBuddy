namespace VocaBuddy.Application.Features.NativeWords.Queries.GetById;

public class GetNativeWordByIdHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetNativeWordByIdQuery, Result<CompactNativeWordDto?>>
{
    private readonly INativeWordRepository _nativeWords = unitOfWork.NativeWords;

    public async Task<Result<CompactNativeWordDto?>> Handle(GetNativeWordByIdQuery request, CancellationToken cancellationToken)
    {
        var nativeWord
            = await _nativeWords.FindByIdAsync(request.WordId, cancellationToken);

        if (nativeWord is null)
        {
            return Result.Failure<CompactNativeWordDto?>(ErrorInfoFactory.NotFound(request.WordId));
        }

        request.EntityUserId = nativeWord.UserId;
        
        return Result.Success(nativeWord.ToCompactNativeWordDto())!;
    }
}
