namespace VocaBuddy.Shared.Errors;

public static class IdentityError
{
    public static class Code
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

    public static class Message
    {
        public const string UserExists = "User with this e-mail address already exists.";
        public const string InvalidCredentials = "Incorrect username or password.";
        public const string UsedUpRefreshToken = "This refresh token has been invalidated.";
        public const string RefreshTokenNotExists = "This refresh token does not exists.";
        public const string NotExpiredToken = "This token hasn not expired yet.";
        public const string JwtIdNotMatch = "The JWT ID of the refresh token does not match the provided JWT.";
        public const string InvalidatedRefreshToken = "This refresh token has been invalidated.";
        public const string ExpiredRefreshToken = "This refresh token has expired.";
        public const string InvalidJwt = "Token is not a JWT with valid security algorithm.";
    }
}
