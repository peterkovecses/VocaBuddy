namespace VocaBuddy.Application.Features.NativeWord.Queries.GetById;

public class GetNativeWordUserIdMatchBehavior(ICurrentUser user) : IPipelineBehavior<GetNativeWordByIdQuery, Result<CompactNativeWordDto>>
{
    private readonly string _currentUserId = user.Id!;

    public async Task<Result<CompactNativeWordDto>> Handle(GetNativeWordByIdQuery request, RequestHandlerDelegate<Result<CompactNativeWordDto>> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (result.IsSuccess && request.EntityUserId != _currentUserId)
        {
            return Result.Failure<CompactNativeWordDto>(ErrorInfoFactory.UserIdNotMatch());
        }

        return result;
    }
}
