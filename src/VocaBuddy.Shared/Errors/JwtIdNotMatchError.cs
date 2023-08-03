namespace VocaBuddy.Shared.Errors;

public class JwtIdNotMatchError : BaseError
{
    public JwtIdNotMatchError() 
        : base(IdentityError.Code.JwtIdNotMatch, IdentityError.Message.JwtIdNotMatch)
    {
    }
}