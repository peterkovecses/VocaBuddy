using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class InvalidatedRefreshTokenException : ApplicationExceptionBase
{
    public InvalidatedRefreshTokenException() : base("This refresh token has been invalidated.") 
    {
        ErrorCode = IdentityErrorCode.InvalidatedRefreshToken;
    }
}
