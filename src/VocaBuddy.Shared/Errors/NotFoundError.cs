namespace VocaBuddy.Shared.Errors;

public class NotFoundError : BaseError
{
    public NotFoundError(string message) : base(VocaBuddyError.Code.NotFound, message)
    {
    }
}