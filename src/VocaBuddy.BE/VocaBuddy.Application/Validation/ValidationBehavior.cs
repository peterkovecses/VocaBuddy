namespace VocaBuddy.Application.Validation;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var allErrors = await GetAllErrors(request, cancellationToken);

        if (allErrors.Count > 0)
        {
            return CreateFailureResponse(ErrorInfoFactory.ValidationError(allErrors));
        }

        return await next();
    }

    private async Task<List<ApplicationError>> GetAllErrors(TRequest request, CancellationToken cancellationToken)
    {
        var allErrors = new List<ApplicationError>();

        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid) continue;
            var errors
                = validationResult
                    .Errors
                    .Select(error => new ApplicationError(
                        error.ErrorMessage,
                        new KeyValuePair<string, object>(error.PropertyName, error.AttemptedValue)));

            allErrors.AddRange(errors);
        }

        return allErrors;
    }

    private static TResponse CreateFailureResponse(ErrorInfo errorInfo)
    {
        var responseType = typeof(TResponse);
        if (!responseType.IsGenericType) return (TResponse)Result.Failure(errorInfo);
        var failureConstructor = responseType.GetConstructor([typeof(ErrorInfo)]);
        var resultInstance = failureConstructor!.Invoke([errorInfo]);

        return (TResponse)resultInstance;
    }
}
