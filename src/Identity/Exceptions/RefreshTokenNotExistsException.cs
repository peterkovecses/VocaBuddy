namespace Identity.Exceptions;

public class RefreshTokenNotExistsException : Exception
{
    public RefreshTokenNotExistsException() : base("This refresh token does not exists.") { }
}
