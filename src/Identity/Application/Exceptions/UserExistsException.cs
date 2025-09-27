namespace Identity.Application.Exceptions;

public class UserExistsException : IdentityExceptionBase
{
    public UserExistsException() : base("User with this e-mail address already exists.") 
    {
        ErrorCode = IdentityErrorCodes.UserExists;
    }
}
