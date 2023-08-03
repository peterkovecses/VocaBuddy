namespace VocaBuddy.Shared.Errors; 

public static class VocaBuddyError 
{
    public static class Code
    {
        public const string Canceled = "Canceled";
        public const string NotFound = "NotFound";
    }

    public static class Message
    {
        public const string Canceled = "Operation was cancelled.";
    }
}
