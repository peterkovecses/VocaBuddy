namespace VocaBuddy.UI.Exceptions;

public class LoginFailedException : Exception
{
	public LoginFailedException(string message) : base($"Login failed: Identity API: {message}")
	{
	}
}
