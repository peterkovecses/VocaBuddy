using VocaBuddy.Shared.Errors;

namespace Identity.Exceptions
{
    public class NotExpiredTokenException : Exception
    {
        public NotExpiredTokenException() : base(IdentityError.NotExpiredTokenMessage) { }
    }
}
