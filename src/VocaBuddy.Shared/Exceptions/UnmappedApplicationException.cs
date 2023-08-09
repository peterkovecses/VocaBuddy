namespace VocaBuddy.Shared.Exceptions;

public class UnmappedApplicationException : Exception
{
    public UnmappedApplicationException(Exception innerException) 
        : base($"ApplicationException was thrown with type {innerException.GetType()}, but a corresponding HTTP status code could not be found. Please check the application's exception handling configuration.",
            innerException)
    {      
    }
}
