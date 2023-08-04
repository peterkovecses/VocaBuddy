using VocaBuddy.Shared.Models;

namespace VocaBuddy.Shared.Errors;

public static class VocaBuddyError
{
    public const string CanceledCode = "Canceled";
    public const string NotFoundCode = "NotFound";

    public const string CanceledMessage = "Operation was cancelled.";

    public static Result Canceled() 
        => new(new ErrorInfo(CanceledCode, CanceledMessage));

    public static Result Notfound(string message) 
        => new(new ErrorInfo(NotFoundCode, message));
}
