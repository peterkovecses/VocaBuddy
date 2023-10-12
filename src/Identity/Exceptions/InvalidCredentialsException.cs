using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class InvalidCredentialsException : IdentityExceptionBase
{
    public InvalidCredentialsException() : base("Incorrect username or password.") 
    {
        ErrorCode = IdentityErrorCode.InvalidCredentials;
    }
}
