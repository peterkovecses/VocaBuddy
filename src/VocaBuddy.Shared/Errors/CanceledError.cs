namespace VocaBuddy.Shared.Errors;

public class CanceledError : BaseError
{
    public CanceledError() : base(VocaBuddyError.Code.Canceled, VocaBuddyError.Message.Canceled)
    {
    }
}
