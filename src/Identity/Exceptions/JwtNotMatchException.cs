namespace Identity.Exceptions;

public class JwtNotMatchException : Exception
{
    public JwtNotMatchException() : base("This refresh token does not match this JWT.") { }
}
