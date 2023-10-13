using System.Text.Json.Serialization;

namespace VocaBuddy.Shared.Errors;

public class ErrorInfo
{
    public string Code { get; }
    public IEnumerable<ApplicationError> Errors { get; }

    public ErrorInfo(string code, ApplicationError error)
    {
        Code = code;
        Errors = new[] { error };
    }

    [JsonConstructor]
    public ErrorInfo(string code, IEnumerable<ApplicationError> errors)
    {
        Code = code;
        Errors = errors;
    }

    public static ErrorInfo ServerError()
        => new("ServerError", new ApplicationError("An error occurred while processing your request."));
}
