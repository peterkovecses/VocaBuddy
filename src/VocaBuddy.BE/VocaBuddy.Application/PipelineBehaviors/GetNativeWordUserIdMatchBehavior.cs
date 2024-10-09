namespace VocaBuddy.Application.PipelineBehaviors;

public class GetNativeWordUserIdMatchBehavior(ICurrentUser user) : IPipelineBehavior<GetNativeWordByIdQuery, Result<NativeWordDto>>
{
    private readonly string _currentUserId = user.Id!;

    public async Task<Result<NativeWordDto>> Handle(GetNativeWordByIdQuery request, RequestHandlerDelegate<Result<NativeWordDto>> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (result.IsSuccess && request.EntityUserId != _currentUserId)
        {
            return Result.Failure<NativeWordDto>(ErrorInfoFactory.UserIdNotMatch());
        }

        return result;
    }
}
