namespace Shared.Exceptions;

public class InvalidUserRegistrationInputException : Exception
{
	public InvalidUserRegistrationInputException(string message) : base(message) { }
}
