namespace VocaBuddy.Shared.Models;

public partial class AuthenticationResult
{

    public AuthenticationResultStatus Status { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? ErrorMessage { get; set; }

    public static AuthenticationResult Success(string token, string refreshToken)
        => new()
        {
            Status = AuthenticationResultStatus.Success,
            Token = token,
            RefreshToken = refreshToken
        };

    public static AuthenticationResult InvalidCredentials(string errorMessage)
        => new()
        {
            Status = AuthenticationResultStatus.InvalidCredentials,
            ErrorMessage = errorMessage
        };

    public static AuthenticationResult Error(string errorMessage)
        => new()
        {
            Status = AuthenticationResultStatus.Error,
            ErrorMessage = errorMessage
        };
}
