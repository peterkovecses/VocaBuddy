namespace Identity.Exceptions;

public class ExpiredRefreshTokenException : IdentityExceptionBase
{
    public ExpiredRefreshTokenException() : base("This refresh token has expired.") 
    {
        ErrorCode = IdentityErrorCode.ExpiredRefreshToken;
    }
}
