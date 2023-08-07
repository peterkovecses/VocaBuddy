namespace VocaBuddy.Shared.Errors;

public static class IdentityErrorCode
{
    public const string UserExists = "UserExists";
    public const string InvalidUserRegistrationInput = "InvalidUserRegistrationInput";
    public const string InvalidCredentials = "InvalidCredentials";
    public const string UsedUpRefreshToken = "UsedUpRefreshToken";
    public const string RefreshTokenNotExists = "RefreshTokenNotExists";
    public const string NotExpiredToken = "NotExpiredToken";
    public const string JwtIdNotMatch = "JwtIdNotMatch";
    public const string InvalidatedRefreshToken = "InvalidatedRefreshToken";
    public const string ExpiredRefreshToken = "ExpiredRefreshToken";
    public const string InvalidJwt = "InvalidJwt";
}
