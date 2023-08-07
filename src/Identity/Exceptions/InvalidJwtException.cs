using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class InvalidJwtException : ApplicationExceptionBase
{
    public InvalidJwtException() : base("Token is not a JWT with valid security algorithm.") 
    {
        ErrorCode = IdentityErrorCode.InvalidJwt;
    }
}
