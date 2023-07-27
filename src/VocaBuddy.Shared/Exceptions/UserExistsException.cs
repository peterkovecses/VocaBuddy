namespace Shared.Exceptions;

public class UserExistsException : Exception
{
    public UserExistsException() : base("User with this e-mail address already exists.") { }
}
