﻿namespace VocaBuddy.Application.Features.NativeWord.Queries.GetLatest;

public class GetLatestNativeWordsWordCountCheckBehavior : IPipelineBehavior<GetLatestNativeWordsQuery, Result<List<NativeWordDto>>>
{
    public async Task<Result<List<NativeWordDto>>> Handle(GetLatestNativeWordsQuery request, RequestHandlerDelegate<Result<List<NativeWordDto>>> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (result.IsSuccess && result.Data!.Count < request.WordCount)
        {
            return Result.Failure<List<NativeWordDto>>(ErrorInfoFactory.ItemCount());
        }

        return result;
    }
}
