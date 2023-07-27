namespace VocaBuddy.Shared.Models;

public sealed class IdentityResult
{
    public const string DefaultErrorMessage = "An error occurred while processing the request.";

    private IdentityResult() {}
    
    public IdentityResultStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
    public TokenHolder? Data { get; init; }

    public static IdentityResult Success()
        => new()
        {
            Status = IdentityResultStatus.Success,
        };

    public static IdentityResult Success(TokenHolder data)
    => new()
    {
        Status = IdentityResultStatus.Success,
        Data = data
    };

    public static IdentityResult InvalidCredentials(string errorMessage)
        => new()
        {
            Status = IdentityResultStatus.InvalidCredentials,
            ErrorMessage = errorMessage
        };

    public static IdentityResult Error(string errorMessage = DefaultErrorMessage)
        => new()
        {
            Status = IdentityResultStatus.Error,
            ErrorMessage = errorMessage
        };
}
