namespace VocaBuddy.Shared.Errors;

public class RefreshTokenNotExistsError : BaseError
{
    public RefreshTokenNotExistsError() 
        : base(IdentityError.Code.RefreshTokenNotExists, IdentityError.Message.RefreshTokenNotExists)
    {
    }
}