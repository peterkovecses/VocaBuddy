using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class InvalidJwtException : Exception
{
    public InvalidJwtException() : base(IdentityError.Message.InvalidJwt) { }
}
