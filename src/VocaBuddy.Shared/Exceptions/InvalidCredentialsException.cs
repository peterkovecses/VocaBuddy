using VocaBuddy.Shared.Errors;

namespace VocaBuddy.Shared.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base(IdentityError.InvalidCredentialsMessage) { }
}
