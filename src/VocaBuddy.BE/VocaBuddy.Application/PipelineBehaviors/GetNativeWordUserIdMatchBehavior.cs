using MediatR;

namespace VocaBuddy.Application.PipelineBehaviors;

public class GetNativeWordUserIdMatchBehavior : IPipelineBehavior<GetNativeWordByIdQuery, Result<NativeWordDto>>
{
    private readonly string _currentUserId;

    public GetNativeWordUserIdMatchBehavior(ICurrentUser user)
    {
        _currentUserId = user.Id!;    
    }

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
