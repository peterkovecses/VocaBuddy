using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class UsedUpRefreshTokenException : Exception
{
    public UsedUpRefreshTokenException() : base(IdentityError.Message.UsedUpRefreshToken) { }
}
