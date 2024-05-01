using MediatR;

namespace VocaBuddy.Application.PipelineBehaviors;

public class GetRandomNativeWordsWordCountCheckBehavior : IPipelineBehavior<GetRandomNativeWordsQuery, Result<List<NativeWordDto>>>
{
    public async Task<Result<List<NativeWordDto>>> Handle(GetRandomNativeWordsQuery request, RequestHandlerDelegate<Result<List<NativeWordDto>>> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (result.IsSuccess && result.Data!.Count < request.WordCount)
        {
            return Result.Failure<List<NativeWordDto>>(ErrorInfoFactory.ItemCount());
        }

        return result;
    }
}
