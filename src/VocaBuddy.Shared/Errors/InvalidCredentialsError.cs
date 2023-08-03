namespace VocaBuddy.Shared.Errors;

public class InvalidCredentialsError : BaseError
{
    public InvalidCredentialsError(string message) 
        : base(IdentityError.Code.InvalidCredentials, message)
    {
    }
}