using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class InvalidUserRegistrationInputException : IdentityExceptionBase
{
	public InvalidUserRegistrationInputException(string message) : base(message) 
	{
		ErrorCode = IdentityErrorCode.InvalidUserRegistrationInput;
	}
}
