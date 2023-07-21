namespace Identity.Exceptions;

public class InvalidJwtException : Exception
{
    public InvalidJwtException() : base("Token is not a JWT with valid security algorithm.") { }
}
