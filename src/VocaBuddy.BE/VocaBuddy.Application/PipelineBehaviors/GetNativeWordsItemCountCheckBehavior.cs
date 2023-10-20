﻿using MediatR;
using VocaBuddy.Application.Errors;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.PipelineBehaviors;

public class GetNativeWordsItemCountCheckBehavior : IPipelineBehavior<GetNativeWordsQuery, Result<List<NativeWordDto>>>
{
    public async Task<Result<List<NativeWordDto>>> Handle(GetNativeWordsQuery request, RequestHandlerDelegate<Result<List<NativeWordDto>>> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if (result.IsSuccess && result.Data!.Count < request.RandomItemCount)
        {
            return Result.Failure<List<NativeWordDto>>(ErrorInfoFactory.ItemCount());
        }

        return result;
    }
}
