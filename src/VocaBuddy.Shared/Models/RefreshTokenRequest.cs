namespace VocaBuddy.Shared.Models;

public class RefreshTokenRequest
{
    public required string AuthToken { get; set; }
    public required string RefreshToken { get; set; }
}
