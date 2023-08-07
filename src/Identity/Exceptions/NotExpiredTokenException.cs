using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace Identity.Exceptions
{
    public class NotExpiredTokenException : ApplicationExceptionBase
    {
        public NotExpiredTokenException() : base("This token hasn not expired yet.") 
        {
            ErrorCode = IdentityErrorCode.NotExpiredToken;
        }
    }
}
