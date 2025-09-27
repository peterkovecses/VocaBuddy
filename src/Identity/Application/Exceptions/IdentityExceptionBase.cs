namespace Identity.Application.Exceptions;

public abstract class IdentityExceptionBase(string message) : Exception(message)
{
    public string ErrorCode { get; protected init; } = "BaseError";
}
