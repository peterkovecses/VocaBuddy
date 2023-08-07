using VocaBuddy.Shared.Errors;

namespace VocaBuddy.Shared.Exceptions;

public class InvalidCredentialsException : ApplicationExceptionBase
{
    public InvalidCredentialsException() : base("Incorrect username or password.") 
    {
        ErrorCode = IdentityErrorCode.InvalidCredentials;
    }
}
