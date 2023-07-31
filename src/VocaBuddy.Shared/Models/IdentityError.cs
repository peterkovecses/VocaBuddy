namespace VocaBuddy.Shared.Models;

public enum IdentityError
{
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
    Server
}