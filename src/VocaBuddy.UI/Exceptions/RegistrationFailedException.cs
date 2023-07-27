namespace VocaBuddy.UI.Exceptions;

public class RegistrationFailedException : Exception
{
	public RegistrationFailedException(string message) : base($"Registration failed: Identity API: {message}")
    {
	}
}
