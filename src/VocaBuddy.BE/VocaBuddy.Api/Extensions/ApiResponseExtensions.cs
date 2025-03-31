namespace VocaBuddy.Api.Extensions;

public static class ApiResponseExtensions
{
    public static IActionResult ToApiResponse(this Result result)
    {
        return result.IsSuccess 
            ? new OkObjectResult(result) 
            : Error(result);
    }

    public static IActionResult ToApiResponse<TData>(this Result<TData> result,
        Func<Result<TData>, ObjectResult>? objectResultGenerator = default)
    {
        if (result.IsSuccess)
        {
            return objectResultGenerator?.Invoke(result) ?? new OkObjectResult(result);
        }

        return Error(result);

    }
    
    private static ObjectResult Error(Result result) =>
        result.ErrorInfo!.Code switch
        {
            VocaBuddyErrorCodes.NotFound => new ObjectResult(result) { StatusCode = StatusCodes.Status404NotFound},
            _ => new BadRequestObjectResult(result)
        };
}
