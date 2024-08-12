namespace Identity.Errors;

public static class ErrorInfoFactory
{
    public static ErrorInfo IdentityError(IdentityExceptionBase exception)
        => new(exception.ErrorCode, [new ApplicationError(exception.Message)]);
}
