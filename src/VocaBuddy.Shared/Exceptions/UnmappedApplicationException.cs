namespace VocaBuddy.Shared.Exceptions;

public class UnmappedApplicationException : Exception
{
    public UnmappedApplicationException(Type type) 
        : base($"ApplicationException was thrown with type {type}, but a corresponding HTTP status code could not be found. Please check the application's exception handling configuration.")
    {      
    }
}
