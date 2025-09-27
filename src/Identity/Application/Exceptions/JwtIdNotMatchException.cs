namespace Identity.Application.Exceptions;

public class JwtIdNotMatchException : IdentityExceptionBase
{
    public JwtIdNotMatchException() : base("The JWT ID of the refresh token does not match the provided JWT.") 
    {
        ErrorCode = IdentityErrorCodes.JwtIdNotMatch;
    }
}
