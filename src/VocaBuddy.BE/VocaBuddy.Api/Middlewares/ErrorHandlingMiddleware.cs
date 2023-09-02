using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Api.Middlewares;

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
        var (code, message) = GetResponseData(exception);
        await SetResponse(context, code, message);
    }

    private (HttpStatusCode, Result) GetResponseData(Exception exception)
    {
        try
        {
            if (exception is not ApplicationExceptionBase appException)
            {
                return exception switch
                {
                    OperationCanceledException => (HttpStatusCode.Accepted, Result.Failure(new ErrorInfo(VocaBuddyErrorCodes.Canceled, "Operation was cancelled."))),
                    DbUpdateException when exception.InnerException is SqlException { Number: 2601 } => (HttpStatusCode.BadRequest, Result.Failure(new ErrorInfo(VocaBuddyErrorCodes.Duplicate, exception.Message))),
                    ValidationException => (HttpStatusCode.BadRequest, Result.Failure(new ErrorInfo(VocaBuddyErrorCodes.ValidationError, exception.Message))),
                    _ => _baseResponseData
                };
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
            NotFoundException => (HttpStatusCode.NotFound),
            UserIdNotMatchException => (HttpStatusCode.Unauthorized),
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