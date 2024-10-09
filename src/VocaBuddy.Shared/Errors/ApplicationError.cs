namespace VocaBuddy.Shared.Errors;

public class ApplicationError(string message, params KeyValuePair<string, object>[] args)
{
    public string Message { get; } = message;
    public KeyValuePair<string, object>[] Args { get; } = args;
}
