namespace EmailDispatcher.Features.SendConfirmationEmail;

public static class SendConfirmationEmailTextTemplate
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