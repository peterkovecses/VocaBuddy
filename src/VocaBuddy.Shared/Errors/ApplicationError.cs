namespace VocaBuddy.Shared.Errors;
public class ApplicationError
{
    public string Message { get; }
    public KeyValuePair<string, object>[] Args { get; } = Array.Empty<KeyValuePair<string, object>>();

    public ApplicationError(string message, params KeyValuePair<string, object>[] args)
    {
        Message = message;
        Args = args;
    }
}
