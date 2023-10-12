using VocaBuddy.Shared.Errors;

namespace VocaBuddy.Application.Errors;

public static class ErrorInfoFactory
{
    public static ErrorInfo NotFound(object id)
        => new(
            VocaBuddyErrorCodes.NotFound,
            new ApplicationError(
                    string.Format("Element with id {0} not found.", id),
                    new KeyValuePair<string, object>(nameof(id), id)));

    public static ErrorInfo UserIdNotMatch()
        => new("UserIdNotMatch", new ApplicationError("The id of the item does not match the id of the current user"));

    public static ErrorInfo ValidationError(List<ApplicationError> errors)
        => new("ValidationError", errors);

    public static ErrorInfo Duplicate(string message)
        => new(VocaBuddyErrorCodes.Duplicate, new ApplicationError(message));
}
