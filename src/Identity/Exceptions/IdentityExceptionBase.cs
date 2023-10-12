namespace Identity.Exceptions;

public abstract class IdentityExceptionBase : Exception
{
    public string ErrorCode { get; init; } = "BaseError";

    public IdentityExceptionBase(string message) : base(message)
    {
    }
}
