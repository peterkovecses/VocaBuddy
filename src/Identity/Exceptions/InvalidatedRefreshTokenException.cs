namespace Identity.Exceptions;

public class InvalidatedRefreshTokenException : Exception
{
    public InvalidatedRefreshTokenException() : base("This refresh token has been invalidated.") { }
}
