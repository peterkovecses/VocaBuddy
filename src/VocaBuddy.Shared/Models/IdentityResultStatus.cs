namespace VocaBuddy.Shared.Models;

public enum IdentityResultStatus
{
    Success,
    UserExists,
    InvalidUserRegistrationInput,
    InvalidCredentials,
    UsedUpRefreshToken,
    RefreshTokenNotExists,
    NotExpiredToken,
    JwtIdNotMatch,
    InvalidatedRefreshToken,
    ExpiredRefreshToken,
    InvalidJwt,
    Error
}