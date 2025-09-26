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
        => new(CommonErrorCodes.ServerError, ApplicationError.ServerError());
    
    public static ErrorInfo ValidationError(KeyValuePair<string, object>[] errors) => 
        new(CommonErrorCodes.ValidationError, ApplicationError.ValidationError(errors));
}
