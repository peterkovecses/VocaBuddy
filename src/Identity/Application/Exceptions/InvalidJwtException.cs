namespace Identity.Application.Exceptions;

public class InvalidJwtException : IdentityExceptionBase
{
    public InvalidJwtException() : base("Token is not a JWT with valid security algorithm.") 
    {
        ErrorCode = IdentityErrorCodes.InvalidJwt;
    }
}
