using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using VocaBuddy.Application.Errors;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var allErrors = await GetAllErrors(request, cancellationToken);

        if (allErrors.Any())
        {
            var errorInfo = ErrorInfoFactory.ValidationError(allErrors);

            _logger.LogError(
                "Request failure {RequestName}, {@Error}, {DateTimeUtc}",
                typeof(TRequest).Name,
                errorInfo,
                DateTime.UtcNow);

            return CreateFailureResponse(errorInfo);
        }

        return await next();
    }

    private async Task<List<ApplicationError>> GetAllErrors(TRequest request, CancellationToken cancellationToken)
    {
        var allErrors = new List<ApplicationError>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors
                    = validationResult
                        .Errors
                        .Select(error => new ApplicationError(
                            error.ErrorMessage,
                            new KeyValuePair<string, object>(error.PropertyName, error.AttemptedValue)));

                allErrors.AddRange(errors);
            }
        }

        return allErrors;
    }

    private static TResponse CreateFailureResponse(ErrorInfo errorInfo)
    {
        var responseType = typeof(TResponse);

        if (responseType.IsGenericType)
        {
            var failureConstructor = responseType.GetConstructor(new[] { typeof(ErrorInfo) });
            var resultInstance = failureConstructor!.Invoke(new object[] { errorInfo });

            return (TResponse)resultInstance;
        }

        return (TResponse)(object)errorInfo;
    }
}
