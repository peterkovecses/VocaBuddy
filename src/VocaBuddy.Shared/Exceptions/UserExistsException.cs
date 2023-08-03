using VocaBuddy.Shared.Errors;

namespace Shared.Exceptions;

public class UserExistsException : Exception
{
    public UserExistsException() : base(IdentityError.Message.UserExists) { }
}
