namespace VocaBuddy.Shared.Errors;

public class UserExistsError : BaseError
{
    public UserExistsError() : base(IdentityError.Code.UserExists, IdentityError.Message.UserExists) { }
}
