﻿using Identity.Exceptions;
using System.Net;
using System.Text.Json;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.Shared.Models;

namespace Identity.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private static readonly (HttpStatusCode, Result) _baseResponseData 
        = (HttpStatusCode.InternalServerError, Result.Failure());

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
        try
        {
            if (exception is not ApplicationExceptionBase appException)
            {
                return _baseResponseData;
            }

            return (GetStatusCode(appException), Result.Failure(appException));
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occured");

            return _baseResponseData;
        }
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
            _ => throw new UnmappedApplicationException(appException)
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
