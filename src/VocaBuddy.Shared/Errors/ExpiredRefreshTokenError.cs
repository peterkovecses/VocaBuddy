namespace VocaBuddy.Shared.Errors;

public class ExpiredRefreshTokenError : BaseError
{
    public ExpiredRefreshTokenError() 
        : base(IdentityError.Code.ExpiredRefreshToken, IdentityError.Message.ExpiredRefreshToken)
    {
    }
}