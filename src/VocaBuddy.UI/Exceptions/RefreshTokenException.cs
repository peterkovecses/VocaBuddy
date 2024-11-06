namespace VocaBuddy.UI.Exceptions;

public class RefreshTokenException(string errors) : Exception($"Failed to refresh token: Identity API: {errors}")
{
}
