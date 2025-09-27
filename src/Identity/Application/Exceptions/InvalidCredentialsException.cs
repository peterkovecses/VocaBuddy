namespace Identity.Application.Exceptions;

public class InvalidCredentialsException : IdentityExceptionBase
{
    public InvalidCredentialsException() : base("Incorrect username or password.") 
    {
        ErrorCode = IdentityErrorCodes.InvalidCredentials;
    }
}
