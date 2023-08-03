namespace VocaBuddy.Shared.Errors;

public class NotExpiredTokenError : BaseError
{
    public NotExpiredTokenError() 
        : base(IdentityError.Code.NotExpiredToken, IdentityError.Message.NotExpiredToken)
    {
    }
}