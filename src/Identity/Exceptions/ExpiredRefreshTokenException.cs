namespace Identity.Exceptions;

public class ExpiredRefreshTokenException : Exception
{
    public ExpiredRefreshTokenException() : base("This refresh token has expired.") { }
}
