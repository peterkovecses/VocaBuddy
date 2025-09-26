namespace EmailDispatcher;

public class Worker(ILogger<Worker> logger, IEmailSender emailSender) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var userRegistered = new UserRegistered
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            ConfirmationLink = "https://example.com/confirm?token=abc123"
        };
        
        var textBody = UserRegisteredTextTemplate.Create(userRegistered.FirstName, userRegistered.ConfirmationLink);
        var message = MimeMessageBuilder
            .StartBuilding()
            .WithSender("VocaBuddy", "info@vocabuddy.com")
            .WithRecipient($"{userRegistered.FirstName} {userRegistered.LastName}", userRegistered.Email)
            .WithSubject("Welcome to VocaBuddy! Please Confirm Your Email")
            .WithBody(textBody)
            .Build();

        await emailSender.SendAsync(message);
    }
}