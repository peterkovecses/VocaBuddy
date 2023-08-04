using System.Net;
using System.Text.Json;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Shared.Errors;
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

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (code, message) = GetResponseData(exception);
        await SetResponse(context, code, message);
    }

    private static (HttpStatusCode, Result) GetResponseData(Exception exception)
        => exception switch
        {
            OperationCanceledException => (HttpStatusCode.Accepted, VocaBuddyError.Canceled()),
            NotFoundException => (HttpStatusCode.NotFound, VocaBuddyError.Notfound(exception.Message)),
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
