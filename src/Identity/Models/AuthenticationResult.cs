namespace Identity.Models;

public class AuthenticationResult
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
