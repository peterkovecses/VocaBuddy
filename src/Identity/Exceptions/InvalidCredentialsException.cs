using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class InvalidCredentialsException : ApplicationExceptionBase
{
    public InvalidCredentialsException() : base("Incorrect username or password.") 
    {
        ErrorCode = IdentityErrorCode.InvalidCredentials;
    }
}
