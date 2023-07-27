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

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (HttpStatusCode code, IdentityResult result) = exception switch
        {   
            // TODO: set the appropiate results
            UserCreationException => (HttpStatusCode.BadRequest, IdentityResult.Error()),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            InvalidCredentialsException => (HttpStatusCode.BadRequest, IdentityResult.InvalidCredentials(exception.Message)),
            InvalidJwtException => (HttpStatusCode.Unauthorized, IdentityResult.Error()),
            UserExistsException=> (HttpStatusCode.Conflict, IdentityResult.Error()),
            _ => (HttpStatusCode.InternalServerError, IdentityResult.Error())
        };

        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}
