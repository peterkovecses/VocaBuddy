namespace VocaBuddy.UI.Exceptions;

public class RefreshTokenException : Exception
{
	public RefreshTokenException(string errors) : base($"Failed to refresh token: Identity API: {errors}")
	{
	}
}
