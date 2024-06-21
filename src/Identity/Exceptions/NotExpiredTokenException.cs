namespace Identity.Exceptions
{
    public class NotExpiredTokenException : IdentityExceptionBase
    {
        public NotExpiredTokenException() : base("This token has not expired yet.") 
        {
            ErrorCode = IdentityErrorCode.NotExpiredToken;
        }
    }
}
