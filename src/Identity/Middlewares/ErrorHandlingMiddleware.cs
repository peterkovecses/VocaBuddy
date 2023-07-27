using Identity.Exceptions;
using Shared.Exceptions;
using System.Net;
using System.Text.Json;
using VocaBuddy.Shared.Exceptions;

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

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (HttpStatusCode code, string message) = exception switch
        {
            UserCreationException => (HttpStatusCode.BadRequest, exception.Message),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized, exception.Message),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized, exception.Message),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized, exception.Message),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
            InvalidJwtException => (HttpStatusCode.Unauthorized, exception.Message),
            UserExistsException=> (HttpStatusCode.Conflict, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An error occurred while processing the request.")
        };

        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(result);
    }
}
