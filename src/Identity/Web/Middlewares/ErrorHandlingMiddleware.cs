namespace Identity.Web.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private static readonly (HttpStatusCode, Result) BaseResponseData 
        = (HttpStatusCode.InternalServerError, Result.ServerError());

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (code, result) = GetResponseData(exception);
        await SetResponse(context, code, result);
    }

    private (HttpStatusCode, Result) GetResponseData(Exception exception)
    {
        try
        {
            return exception is not IdentityExceptionBase identityException
                ? BaseResponseData
                : (GetStatusCode(identityException), Result.Failure(ErrorInfoFactory.IdentityError(identityException)));
        }
        
        catch (Exception ex)
        {
            logger.LogError(ex, ErrorMessages.ExceptionOccurred);

            return BaseResponseData;
        }
    }

    private static HttpStatusCode GetStatusCode(IdentityExceptionBase appException)
    {
        return appException switch
        {
            UserExistsException => HttpStatusCode.Conflict,
            InvalidUserRegistrationInputException => HttpStatusCode.BadRequest,
            InvalidCredentialsException => HttpStatusCode.BadRequest,
            UsedUpRefreshTokenException => HttpStatusCode.Unauthorized,
            RefreshTokenNotExistsException => HttpStatusCode.Unauthorized,
            NotExpiredTokenException => HttpStatusCode.Unauthorized,
            JwtIdNotMatchException => HttpStatusCode.Unauthorized,
            InvalidatedRefreshTokenException => HttpStatusCode.Unauthorized,
            ExpiredRefreshTokenException => HttpStatusCode.Unauthorized,
            InvalidJwtException => HttpStatusCode.Unauthorized,
            _ => throw new UnmappedIdentityException(appException)
        };
    }

    private static async Task SetResponse(HttpContext context, HttpStatusCode code, Result result)
    {
        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}
