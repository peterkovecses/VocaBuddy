
using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions;

public class JwtIdNotMatchException : Exception
{
    public JwtIdNotMatchException() : base(IdentityError.JwtIdNotMatchMessage) { }
}
