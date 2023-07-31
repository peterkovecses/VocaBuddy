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
        var (code, result) = GetResponseData(exception, exception.Message);
        await SetResponse(context, code, result);
    }

    private static (HttpStatusCode, IdentityResult) GetResponseData(Exception exception, string message)
        => exception switch
        {
            UserExistsException => (HttpStatusCode.Conflict, IdentityResult.FromError(IdentityError.UserExists, message)),
            InvalidUserRegistrationInputException => (HttpStatusCode.BadRequest, IdentityResult.FromError(IdentityError.InvalidUserRegistrationInput, message)),
            InvalidCredentialsException => (HttpStatusCode.BadRequest, IdentityResult.FromError(IdentityError.InvalidCredentials, message)),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.UsedUpRefreshToken, message)),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.RefreshTokenNotExists, message)),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.NotExpiredToken, message)),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.JwtIdNotMatch, message)),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.InvalidatedRefreshToken, message)),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.ExpiredRefreshToken, message)),
            InvalidJwtException => (HttpStatusCode.Unauthorized, IdentityResult.FromError(IdentityError.InvalidJwt, message)),
            _ => (HttpStatusCode.InternalServerError, IdentityResult.ServerError())
        };

    private static async Task SetResponse(HttpContext context, HttpStatusCode code, IdentityResult result)
    {
        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}
