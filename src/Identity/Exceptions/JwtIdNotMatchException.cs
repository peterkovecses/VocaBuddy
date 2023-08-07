
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions;

public class JwtIdNotMatchException : ApplicationExceptionBase
{
    public JwtIdNotMatchException() : base("The JWT ID of the refresh token does not match the provided JWT.") 
    {
        ErrorCode = IdentityErrorCode.JwtIdNotMatch;
    }
}
