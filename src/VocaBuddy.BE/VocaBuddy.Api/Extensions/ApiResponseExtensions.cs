using Microsoft.AspNetCore.Mvc;

namespace VocaBuddy.Api.Extensions;

public static class ApiResponseExtensions
{
    public static IActionResult ToApiResponse(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result);
        }

        return result.ErrorInfo!.Code switch
        {
            VocaBuddyErrorCodes.NotFound => new NotFoundResult(),
            _ => new BadRequestObjectResult(result)
        };
    }

    public static IActionResult ToApiResponse<TData>(this Result<TData> result,
        Func<Result<TData>, ObjectResult>? objectResultGenerator = default)
    {
        if (result.IsSuccess)
        {
            return objectResultGenerator?.Invoke(result) ?? new OkObjectResult(result);
        }

        return result.ErrorInfo!.Code switch
        {
            VocaBuddyErrorCodes.NotFound => new NotFoundResult(),
            _ => new BadRequestObjectResult(result)
        };
    }
}
