using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace VocaBuddy.UI.Exceptions;

public class RefreshTokenException : ApplicationExceptionBase
{
	public RefreshTokenException(ErrorInfo error) : base($"Failed to refresh token: Identity API: {error.Message}")
	{
		ErrorCode = error.Code;
	}
}
