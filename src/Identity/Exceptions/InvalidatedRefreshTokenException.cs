using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class InvalidatedRefreshTokenException : Exception
{
    public InvalidatedRefreshTokenException() : base(IdentityError.InvalidatedRefreshTokenMessage) { }
}
