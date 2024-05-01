namespace Identity.Exceptions
{
    public class NotExpiredTokenException : IdentityExceptionBase
    {
        public NotExpiredTokenException() : base("This token hasn not expired yet.") 
        {
            ErrorCode = IdentityErrorCode.NotExpiredToken;
        }
    }
}
