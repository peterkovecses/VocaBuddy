namespace VocaBuddy.Shared.Models;

public sealed class IdentityResult
{
    public const string DefaultErrorMessage = "An error occurred while processing the request.";

    private IdentityResult() { }

    public IdentityResultStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
    public TokenHolder? Tokens { get; init; }

    public static IdentityResult Success()
        => new()
        {
            Status = IdentityResultStatus.Success,
        };

    public static IdentityResult Success(TokenHolder data)
        => new()
        {
            Status = IdentityResultStatus.Success,
            Tokens = data
        };

    public static IdentityResult UserExists(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.UserExists,
            ErrorMessage = errorMessage
        };

    public static IdentityResult InvalidUserRegistrationInput(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.InvalidUserRegistrationInput,
            ErrorMessage = errorMessage
        };

    public static IdentityResult InvalidCredentials(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.InvalidCredentials,
            ErrorMessage = errorMessage
        };

    public static IdentityResult UsedUpRefreshToken(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.UsedUpRefreshToken,
            ErrorMessage = errorMessage
        };

    public static IdentityResult RefreshTokenNotExists(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.RefreshTokenNotExists,
            ErrorMessage = errorMessage
        };
    
    public static IdentityResult NotExpiredToken(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.NotExpiredToken,
            ErrorMessage = errorMessage
        };
    
    public static IdentityResult JwtIdNotMatch(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.JwtIdNotMatch,
            ErrorMessage = errorMessage
        };
    
    public static IdentityResult InvalidatedRefreshToken(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.InvalidatedRefreshToken,
            ErrorMessage = errorMessage
        };

    public static IdentityResult ExpiredRefreshToken(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.ExpiredRefreshToken,
            ErrorMessage = errorMessage
        };

    public static IdentityResult InvalidJwt(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.InvalidJwt,
            ErrorMessage = errorMessage
        };

    public static IdentityResult Error(string errorMessage = DefaultErrorMessage)
        => new()
        {
            Status = IdentityResultStatus.Error,
            ErrorMessage = errorMessage
        };
}
