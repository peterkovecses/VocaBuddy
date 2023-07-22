namespace Shared.Exceptions;

public class UserCreationException : Exception
{
	public UserCreationException(string message) : base(message) { }
}
