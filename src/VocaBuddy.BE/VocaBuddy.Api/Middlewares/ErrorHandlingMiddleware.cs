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

        return exception switch
        {
            OperationCanceledException => (HttpStatusCode.Accepted, Result.CustomError(new ErrorInfo(VocaBuddyErrorCodes.Canceled, "Operation was cancelled."))),
            _ => baseResponseData
        };
    }

    private static HttpStatusCode GetStatusCode(ApplicationExceptionBase appException)
    {
        return appException switch
        {
            NotFoundException => (HttpStatusCode.NotFound),
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