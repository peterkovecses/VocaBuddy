namespace Identity.Application.Exceptions;

public class InvalidUserRegistrationInputException : IdentityExceptionBase
{
	public InvalidUserRegistrationInputException(string message) : base(message) 
	{
		ErrorCode = IdentityErrorCodes.InvalidUserRegistrationInput;
	}
}