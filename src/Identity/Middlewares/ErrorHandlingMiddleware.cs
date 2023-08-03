using Identity.Exceptions;
using Shared.Exceptions;
using System.Net;
using System.Text.Json;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.Shared.Interfaces;
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

    private static (HttpStatusCode, Result<BaseError>) GetResponseData(Exception exception)
        => exception switch
        {
            UserExistsException => (HttpStatusCode.Conflict, Result.Failure(new UserExistsError())),
            InvalidUserRegistrationInputException => (HttpStatusCode.BadRequest, Result.Failure(new InvalidUserRegistrationInputError(exception.Message))),
            InvalidCredentialsException => (HttpStatusCode.BadRequest, Result.Failure(new InvalidCredentialsError(exception.Message))),
            UsedUpRefreshTokenException => (HttpStatusCode.Unauthorized, Result.Failure(new UsedUpRefreshTokenError())),
            RefreshTokenNotExistsException => (HttpStatusCode.Unauthorized, Result.Failure(new RefreshTokenNotExistsError())),
            NotExpiredTokenException => (HttpStatusCode.Unauthorized, Result.Failure(new NotExpiredTokenError())),
            JwtIdNotMatchException => (HttpStatusCode.Unauthorized, Result.Failure(new JwtIdNotMatchError())),
            InvalidatedRefreshTokenException => (HttpStatusCode.Unauthorized, Result.Failure(new InvalidatedRefreshTokenError())),
            ExpiredRefreshTokenException => (HttpStatusCode.Unauthorized, Result.Failure(new ExpiredRefreshTokenError())),
            InvalidJwtException => (HttpStatusCode.Unauthorized, Result.Failure(new InvalidJwtError())),
            _ => (HttpStatusCode.InternalServerError, Result.Failure(new BaseError()))
        };

    private static async Task SetResponse(HttpContext context, HttpStatusCode code, Result<BaseError> result)
    {
        var jsonContent = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(jsonContent);
    }
}
