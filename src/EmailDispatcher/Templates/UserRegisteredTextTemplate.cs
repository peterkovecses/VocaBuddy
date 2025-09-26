namespace EmailDispatcher.Templates;

public static class UserRegisteredTextTemplate
{
    public static string Create(string firstName, string confirmationLink) 
        => $""" 
            Hi {firstName},
            
            Welcome to VocaBuddy!
            
            Please confirm your email by clicking the link below:
            {confirmationLink}

            Thank you!
            """;
}