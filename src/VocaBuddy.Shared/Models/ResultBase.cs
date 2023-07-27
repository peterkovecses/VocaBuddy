namespace VocaBuddy.Shared.Models;

public abstract class ResultBase
{
    public const string DefaultErrorMessage = "An error occurred while processing the request.";

    public string? ErrorMessage { get; set; }
}
