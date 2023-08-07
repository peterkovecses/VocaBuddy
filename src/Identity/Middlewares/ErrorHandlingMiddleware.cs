using Identity.Exceptions;
using Shared.Exceptions;
using System.Net;
using System.Text.Json;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.Shared.Models;

namespace Identity.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occured");
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
        var baseResponseData = (HttpStatusCode.InternalServerError, Result.BaseError());

        if (exception is ApplicationExceptionBase appException)
        {
            var result = Result.FromException(appException);

            try
            {
                HttpStatusCode code = GetStatusCode(appException);

                return (code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured");

                return baseResponseData;
            }
        }

        return baseResponseData;
    }

    private static HttpStatusCode GetStatusCode(ApplicationExceptionBase appException)
    {
        return appException switch
        {
            UserExistsException => (HttpStatusCode.Conflict),
            InvalidUserRegistrationInputException => (HttpStatusCode.BadRequest),
            InvalidCredentialsException => (HttpStatusCode.BadRequest),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized),
            InvalidJwtException => (HttpStatusCode.Unauthorized),
            _ => throw new UnmappedApplicationException(appException.GetType())
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
