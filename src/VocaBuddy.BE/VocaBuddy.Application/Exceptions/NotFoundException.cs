using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace VocaBuddy.Application.Exceptions;

public class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException(int id) : base($"Item with id {id} not found.")
    {
        ErrorCode = VocaBuddyErrorCodes.NotFound;
    }
}
