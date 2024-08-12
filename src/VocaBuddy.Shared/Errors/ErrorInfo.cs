namespace VocaBuddy.Shared.Errors;

[method: Newtonsoft.Json.JsonConstructor]
public class ErrorInfo(string code, IEnumerable<ApplicationError> errors)
{
    public string Code { get; } = code;
    public IEnumerable<ApplicationError> Errors { get; } = errors;

    public static ErrorInfo ServerError()
        => new("ServerError", [new ApplicationError("An error occurred while processing your request.")]);
}
