using Identity.Exceptions;
using Shared.Exceptions;
using System.Net;
using System.Text.Json;
using VocaBuddy.Shared.Errors;
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
        var (code, result) = GetResponseData(exception);
        await SetResponse(context, code, result);
    }

    private static (HttpStatusCode, Result) GetResponseData(Exception exception)
        => exception switch
        {
            UserExistsException => (HttpStatusCode.Conflict, IdentityError.UserExists()),
            InvalidUserRegistrationInputException => (HttpStatusCode.BadRequest, IdentityError.InvalidUserRegistrationInput(exception.Message)),
            InvalidCredentialsException => (HttpStatusCode.BadRequest, IdentityError.InvalidCredentials()),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityError.UsedUpRefreshToken()),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized, IdentityError.RefreshTokenNotExists()),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized, IdentityError.NotExpiredToken()),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized, IdentityError.JwtIdNotMatch()),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityError.InvalidatedRefreshToken()),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized, IdentityError.ExpiredRefreshToken()),
            InvalidJwtException => (HttpStatusCode.Unauthorized, IdentityError.InvalidJwt()),
            _ => (HttpStatusCode.InternalServerError, Result.BaseError())
        };

    private static async Task SetResponse(HttpContext context, HttpStatusCode code, Result result)
    {
        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}
