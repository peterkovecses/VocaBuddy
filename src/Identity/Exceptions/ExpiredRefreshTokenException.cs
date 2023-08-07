using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class ExpiredRefreshTokenException : ApplicationExceptionBase
{
    public ExpiredRefreshTokenException() : base("This refresh token has expired.") 
    {
        ErrorCode = IdentityErrorCode.ExpiredRefreshToken;
    }
}
