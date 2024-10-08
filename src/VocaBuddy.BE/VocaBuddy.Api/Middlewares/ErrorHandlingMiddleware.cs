namespace VocaBuddy.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorMessages.ExceptionOccurred);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (code, message) = GetResponseData(exception);
        await SetResponse(context, code, message);
    }

    private static (HttpStatusCode, Result) GetResponseData(Exception exception)
        => exception switch
        {
            OperationCanceledException => (HttpStatusCode.Accepted, Result.Failure(ErrorInfoFactory.Canceled())),
            DbUpdateException when exception.InnerException is SqlException { Number: 2601 } => (HttpStatusCode.BadRequest, Result.Failure(ErrorInfoFactory.Duplicate(exception.Message))),
            _ => (HttpStatusCode.InternalServerError, Result.ServerError())
        };


    private static async Task SetResponse(HttpContext context, HttpStatusCode code, Result result)
    {
        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}