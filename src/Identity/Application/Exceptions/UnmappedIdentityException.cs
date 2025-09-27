namespace Identity.Application.Exceptions;

public class UnmappedIdentityException : Exception
{
    public UnmappedIdentityException(Exception innerException)
        : base($"ApplicationException was thrown with type {innerException.GetType()}, but a corresponding HTTP status code could not be found. Please check the application's exception handling configuration.",
            innerException)
    {
    }
}
