namespace Identity.Exceptions;

public class UserExistsException : IdentityExceptionBase
{
    public UserExistsException() : base("User with this e-mail address already exists.") 
    {
        ErrorCode = IdentityErrorCode.UserExists;
    }
}
