using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class ExpiredRefreshTokenException : Exception
{
    public ExpiredRefreshTokenException() : base(IdentityError.ExpiredRefreshTokenMessage) { }
}
