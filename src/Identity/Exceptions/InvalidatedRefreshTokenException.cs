namespace Identity.Exceptions;

public class InvalidatedRefreshTokenException : IdentityExceptionBase
{
    public InvalidatedRefreshTokenException() : base("This refresh token has been invalidated.") 
    {
        ErrorCode = IdentityErrorCode.InvalidatedRefreshToken;
    }
}
