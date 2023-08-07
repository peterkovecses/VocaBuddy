using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class RefreshTokenNotExistsException : ApplicationExceptionBase
{
    public RefreshTokenNotExistsException() : base("This refresh token does not exists.")
    {
        ErrorCode = IdentityErrorCode.RefreshTokenNotExists;
    }
}
