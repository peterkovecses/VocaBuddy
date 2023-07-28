namespace VocaBuddy.UI.Exceptions;

public class RefreshTokenException : Exception
{
	public RefreshTokenException(string message) : base($"Failed to refresh token: Identity API: {message}")
	{

	}
}
