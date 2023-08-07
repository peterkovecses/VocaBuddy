using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class UsedUpRefreshTokenException : ApplicationExceptionBase
{
    public UsedUpRefreshTokenException() : base("This refresh token has already been used.") 
    {
        ErrorCode = IdentityErrorCode.UsedUpRefreshToken;
    }
}
