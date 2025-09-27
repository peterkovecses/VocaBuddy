namespace Identity.Application.Exceptions;

public class ExpiredRefreshTokenException : IdentityExceptionBase
{
    public ExpiredRefreshTokenException() : base("This refresh token has expired.") 
    {
        ErrorCode = IdentityErrorCodes.ExpiredRefreshToken;
    }
}
