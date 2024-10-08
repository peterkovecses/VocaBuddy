namespace VocaBuddy.Shared.Errors;

public class ErrorInfo
{
    public string Code { get; }
    public IEnumerable<ApplicationError> Errors { get; }

    [Newtonsoft.Json.JsonConstructor]
    public ErrorInfo(string code, IEnumerable<ApplicationError> errors)
    {
        Code = code;
        Errors = errors;
    }

    public ErrorInfo(string code, ApplicationError error)
    {
        Code = code;
        Errors = [error];
    }

    public static ErrorInfo ServerError()
        => new("ServerError", new ApplicationError("An error occurred while processing your request."));
}
