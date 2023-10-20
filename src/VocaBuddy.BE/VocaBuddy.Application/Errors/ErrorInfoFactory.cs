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
        => new(VocaBuddyErrorCodes.UserIdNotMatch, new ApplicationError("The id of the item does not match the id of the current user"));

    public static ErrorInfo ValidationError(List<ApplicationError> errors)
        => new(VocaBuddyErrorCodes.ValidationError, errors);

    public static ErrorInfo Duplicate(string message)
        => new(VocaBuddyErrorCodes.Duplicate, new ApplicationError(message));

    public static ErrorInfo Canceled()
        => new(VocaBuddyErrorCodes.Canceled, new ApplicationError("Operation was cancelled."));

    public static ErrorInfo ItemCount()
        => new(VocaBuddyErrorCodes.ItemCount, new ApplicationError("There are fewer elements in the db than requested."));
}
