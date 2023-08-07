namespace VocaBuddy.Shared.Exceptions;

public abstract class ApplicationExceptionBase : Exception
{
    public string ErrorCode { get; init; } = "BaseError";

    public ApplicationExceptionBase(string message) : base(message)
    {        
    }
}
