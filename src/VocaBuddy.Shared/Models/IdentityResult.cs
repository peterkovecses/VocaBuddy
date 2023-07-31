namespace VocaBuddy.Shared.Models;

public sealed class IdentityResult
{
    public const string DefaultErrorMessage = "An error occurred while processing the request.";

    private IdentityResult() { }

    public IdentityError? Error { get; init; }
    public string? ErrorMessage { get; init; }
    public TokenHolder? Tokens { get; init; }
    public bool IsSuccess => Error is null;
    public bool IsError => !IsSuccess;

    public static IdentityResult Success()
        => new();

    public static IdentityResult Success(TokenHolder tokens)
        => new()
        {
            Tokens = tokens
        };

    public static IdentityResult UserExists(string errorMessage)
        => new()
        {
            Error = IdentityError.UserExists,
            ErrorMessage = errorMessage
        };

    public static IdentityResult InvalidUserRegistrationInput(string errorMessage)
        => new()
        {
            Error = IdentityError.InvalidUserRegistrationInput,
            ErrorMessage = errorMessage
        };

    public static IdentityResult InvalidCredentials(string errorMessage)
        => new()
        {
            Error = IdentityError.InvalidCredentials,
            ErrorMessage = errorMessage
        };

    public static IdentityResult UsedUpRefreshToken(string errorMessage)
        => new()
        {
            Error = IdentityError.UsedUpRefreshToken,
            ErrorMessage = errorMessage
        };

    public static IdentityResult RefreshTokenNotExists(string errorMessage)
        => new()
        {
            Error = IdentityError.RefreshTokenNotExists,
            ErrorMessage = errorMessage
        };
    
    public static IdentityResult NotExpiredToken(string errorMessage)
        => new()
        {
            Error = IdentityError.NotExpiredToken,
            ErrorMessage = errorMessage
        };
    
    public static IdentityResult JwtIdNotMatch(string errorMessage)
        => new()
        {
            Error = IdentityError.JwtIdNotMatch,
            ErrorMessage = errorMessage
        };
    
    public static IdentityResult InvalidatedRefreshToken(string errorMessage)
        => new()
        {
            Error = IdentityError.InvalidatedRefreshToken,
            ErrorMessage = errorMessage
        };

    public static IdentityResult ExpiredRefreshToken(string errorMessage)
        => new()
        {
            Error = IdentityError.ExpiredRefreshToken,
            ErrorMessage = errorMessage
        };

    public static IdentityResult InvalidJwt(string errorMessage)
        => new()
        {
            Error = IdentityError.InvalidJwt,
            ErrorMessage = errorMessage
        };

    public static IdentityResult Unknown(string errorMessage = DefaultErrorMessage)
        => new()
        {
            Error = IdentityError.Unknown,
            ErrorMessage = errorMessage
        };
}
