namespace Identity.Exceptions
{
    public class NotExpiredTokenException : Exception
    {
        public NotExpiredTokenException() : base("This token hasn not expired yet.") { }
    }
}
