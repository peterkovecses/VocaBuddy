namespace VocaBuddy.Shared.Errors;

public class InvalidUserRegistrationInputError : BaseError
{
    public InvalidUserRegistrationInputError(string message) 
        : base(IdentityError.Code.InvalidUserRegistrationInput, message)
    {
    }
}