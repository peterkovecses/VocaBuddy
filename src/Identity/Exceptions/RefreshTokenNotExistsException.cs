namespace Identity.Exceptions;

public class RefreshTokenNotExistsException : IdentityExceptionBase
{
    public RefreshTokenNotExistsException() : base("This refresh token does not exists.")
    {
        ErrorCode = IdentityErrorCode.RefreshTokenNotExists;
    }
}
