namespace Identity.Exceptions;

public class JwtIdNotMatchException : Exception
{
    public JwtIdNotMatchException() : base("The JWT ID of the refresh token does not match the provided JWT.") { }
}
