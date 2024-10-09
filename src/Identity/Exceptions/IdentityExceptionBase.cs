namespace Identity.Exceptions;

public abstract class IdentityExceptionBase : Exception
{
    public string ErrorCode { get; init; } = "BaseError";

    protected IdentityExceptionBase(string message) : base(message)
    {
    }
}
