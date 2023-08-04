using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class RefreshTokenNotExistsException : Exception
{
    public RefreshTokenNotExistsException() : base(IdentityError.RefreshTokenNotExistsMessage) { }
}
