namespace VocaBuddy.Shared.Errors;

public class UsedUpRefreshTokenError : BaseError
{
    public UsedUpRefreshTokenError() 
        : base(IdentityError.Code.UsedUpRefreshToken, IdentityError.Message.UsedUpRefreshToken)
    {
    }
}