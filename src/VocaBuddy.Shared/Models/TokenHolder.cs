namespace VocaBuddy.Shared.Models;

public class TokenHolder
{
    public required string AuthToken { get; set; }
    public required string RefreshToken { get; set; }       
}
