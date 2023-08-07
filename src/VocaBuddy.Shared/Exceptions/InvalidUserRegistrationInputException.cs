using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Shared.Exceptions;

public class InvalidUserRegistrationInputException : ApplicationExceptionBase
{
	public InvalidUserRegistrationInputException(string message) : base(message) 
	{
		ErrorCode = IdentityErrorCode.InvalidUserRegistrationInput;
	}
}
