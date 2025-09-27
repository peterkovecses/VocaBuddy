namespace Identity.Application.Exceptions;

public class UsedUpRefreshTokenException : IdentityExceptionBase
{
    public UsedUpRefreshTokenException() : base("This refresh token has already been used.") 
    {
        ErrorCode = IdentityErrorCodes.UsedUpRefreshToken;
    }
}
