namespace VocaBuddy.Shared.Errors;

public class InvalidJwtError : BaseError
{
    public InvalidJwtError() : base(IdentityError.Code.InvalidJwt, IdentityError.Message.InvalidJwt)
    {
    }
}