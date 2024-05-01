namespace Identity.Exceptions;

public class UsedUpRefreshTokenException : IdentityExceptionBase
{
    public UsedUpRefreshTokenException() : base("This refresh token has already been used.") 
    {
        ErrorCode = IdentityErrorCode.UsedUpRefreshToken;
    }
}
