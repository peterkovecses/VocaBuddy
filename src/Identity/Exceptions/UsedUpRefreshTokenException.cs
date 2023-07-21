namespace Identity.Exceptions;

public class UsedUpRefreshTokenException : Exception
{
    public UsedUpRefreshTokenException() : base("This refresh token has been invalidated.") { }
}
