using MediatR;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.PipelineBehaviors;

public class GetNativeWordUserIdMatchBehavior : IPipelineBehavior<GetNativeWordByIdQuery, NativeWordDto>
{
    private readonly string _currentUserId;

    public GetNativeWordUserIdMatchBehavior(ICurrentUser user)
    {
        _currentUserId = user.Id!;    
    }

    public async Task<NativeWordDto> Handle(GetNativeWordByIdQuery request, RequestHandlerDelegate<NativeWordDto> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (request.EntityUserId != _currentUserId)
        {
            throw new UserIdNotMatchException();
        }

        return result;
    }
}
