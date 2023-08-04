using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.Shared.Errors;

public class ErrorInfo
{
    private const string BaseMessage = "An error occurred while processing the request.";
    private const string BaseCode = "BaseError";

    public string Code { get; init; }
    public string Message { get; init; }

    public ErrorInfo(string code = BaseCode, string message = BaseMessage)
    {
        Code = code;
        Message = message;
    }
}
