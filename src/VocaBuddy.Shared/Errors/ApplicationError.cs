namespace VocaBuddy.Shared.Errors;

public class ApplicationError(string message, params KeyValuePair<string, object>[] args)
{
    public string Message { get; } = message;
    public KeyValuePair<string, object>[] Args { get; } = args;
    
    public static ApplicationError ServerError() 
        => new("An error occurred while processing your request.");
    
    public static ApplicationError ValidationError(KeyValuePair<string, object>[] errors) 
        => new("One or more validation errors occurred.", errors);
}
