using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace VocaBuddy.Application.Exceptions;

public class UserIdNotMatchException : ApplicationExceptionBase
{
    public UserIdNotMatchException() : base("The id of the item does not match the id of the current user")
    {
        ErrorCode = VocaBuddyErrorCodes.UserIdNotMatch;
    }
}
