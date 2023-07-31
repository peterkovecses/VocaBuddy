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
        var message = exception.Message;

        (HttpStatusCode code, IdentityResult result) = exception switch
        {
            UserExistsException => (HttpStatusCode.Conflict, IdentityResult.UserExists(message)),
            InvalidUserRegistrationInputException => (HttpStatusCode.BadRequest, IdentityResult.InvalidUserRegistrationInput(message)),
            InvalidCredentialsException => (HttpStatusCode.BadRequest, IdentityResult.InvalidCredentials(message)),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.UsedUpRefreshToken(message)),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized, IdentityResult.RefreshTokenNotExists(message)),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized, IdentityResult.NotExpiredToken(message)),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized, IdentityResult.JwtIdNotMatch(message)),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.InvalidatedRefreshToken(message)),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.ExpiredRefreshToken(message)),
            InvalidJwtException => (HttpStatusCode.Unauthorized, IdentityResult.InvalidJwt(message)),
            _ => (HttpStatusCode.InternalServerError, IdentityResult.Unknown())
        };

        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}
