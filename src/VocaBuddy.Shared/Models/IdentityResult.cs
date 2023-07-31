namespace VocaBuddy.Shared.Models;

public sealed class IdentityResult
{
    public const string ServerErrorMessage = "An error occurred while processing the request.";

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

    public static IdentityResult FromError(IdentityError error, string errorMessage)
        => new()
        {
            Error = error,
            ErrorMessage = errorMessage
        };

    public static IdentityResult ServerError()
        => new()
        {
            Error = IdentityError.Server,
            ErrorMessage = ServerErrorMessage
        };
}
