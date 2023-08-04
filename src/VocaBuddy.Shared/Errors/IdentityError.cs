using VocaBuddy.Shared.Models;

namespace VocaBuddy.Shared.Errors;

public static class IdentityError
{
    public const string UserExistsCode = "UserExists";
    public const string InvalidUserRegistrationInputCode = "InvalidUserRegistrationInput";
    public const string InvalidCredentialsCode = "InvalidCredentials";
    public const string UsedUpRefreshTokenCode = "UsedUpRefreshToken";
    public const string RefreshTokenNotExistsCode = "RefreshTokenNotExists";
    public const string NotExpiredTokenCode = "NotExpiredToken";
    public const string JwtIdNotMatchCode = "JwtIdNotMatch";
    public const string InvalidatedRefreshTokenCode = "InvalidatedRefreshToken";
    public const string ExpiredRefreshTokenCode = "ExpiredRefreshToken";
    public const string InvalidJwtCode = "InvalidJwt";

    public const string UserExistsMessage = "User with this e-mail address already exists.";
    public const string InvalidCredentialsMessage = "Incorrect username or password.";
    public const string UsedUpRefreshTokenMessage = "This refresh token has been invalidated.";
    public const string RefreshTokenNotExistsMessage = "This refresh token does not exists.";
    public const string NotExpiredTokenMessage = "This token hasn not expired yet.";
    public const string JwtIdNotMatchMessage = "The JWT ID of the refresh token does not match the provided JWT.";
    public const string InvalidatedRefreshTokenMessage = "This refresh token has been invalidated.";
    public const string ExpiredRefreshTokenMessage = "This refresh token has expired.";
    public const string InvalidJwtMessage = "Token is not a JWT with valid security algorithm.";

    public static Result UserExists() 
        => new(new ErrorInfo(UserExistsCode, UserExistsMessage));

    public static Result InvalidUserRegistrationInput(string message) 
        => new(new ErrorInfo(InvalidUserRegistrationInputCode, message));

    public static Result InvalidCredentials() 
        => new(new ErrorInfo(InvalidCredentialsCode, InvalidCredentialsMessage));

    public static Result UsedUpRefreshToken() 
        => new(new ErrorInfo(UsedUpRefreshTokenCode, UsedUpRefreshTokenMessage));

    public static Result RefreshTokenNotExists() 
        => new(new ErrorInfo(RefreshTokenNotExistsCode, RefreshTokenNotExistsMessage));

    public static Result NotExpiredToken() 
        => new(new ErrorInfo(NotExpiredTokenCode, NotExpiredTokenMessage));

    public static Result JwtIdNotMatch() 
        => new(new ErrorInfo(JwtIdNotMatchCode, JwtIdNotMatchMessage));

    public static Result InvalidatedRefreshToken() 
        => new(new ErrorInfo(InvalidatedRefreshTokenCode, InvalidatedRefreshTokenMessage));

    public static Result ExpiredRefreshToken() 
        => new(new ErrorInfo(ExpiredRefreshTokenCode, ExpiredRefreshTokenMessage));

    public static Result InvalidJwt() 
        => new(new ErrorInfo(InvalidJwtCode, InvalidJwtMessage));
}
