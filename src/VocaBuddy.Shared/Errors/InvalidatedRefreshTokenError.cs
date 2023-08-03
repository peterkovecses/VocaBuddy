namespace VocaBuddy.Shared.Errors;

public class InvalidatedRefreshTokenError : BaseError
{
    public InvalidatedRefreshTokenError() 
        : base(IdentityError.Code.InvalidatedRefreshToken, IdentityError.Message.InvalidatedRefreshToken)
    {
    }
}