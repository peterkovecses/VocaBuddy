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
            UsedUpRefreshTokenException => (HttpStatusCode.BadRequest, exception.Message),
            RefreshTokenNotExistsException => (HttpStatusCode.BadRequest, exception.Message),
            NotExpiredTokenException => (HttpStatusCode.BadRequest, exception.Message),
            JwtNotMatchException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidatedRefreshTokenException => (HttpStatusCode.BadRequest, exception.Message),
            ExpiredRefreshTokenException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidCredentialsException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidJwtException => (HttpStatusCode.BadRequest, exception.Message),
            UserExistsException=> (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An error occurred while processing the request.")
        };

        var result = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(result);
    }
}
