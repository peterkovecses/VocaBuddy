namespace EmailDispatcher.Features.SendConfirmationEmail;

public class SendConfirmationEmailHandler(IEmailSender emailSender) : IRequestHandler<SendConfirmationEmailCommand, Unit>
{
    public async Task<Unit> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        var textBody = UserRegisteredTextTemplate.Create(request.FirstName, request.ConfirmationLink);
        var message = MimeMessageBuilder
            .StartBuilding()
            .WithMessageId(request.Id)
            .WithSender("VocaBuddy Support", "support@vocabuddy.com")
            .WithRecipient(request.FullName, request.Email)
            .WithSubject("Welcome to VocaBuddy! Please Confirm Your Email")
            .WithBody(textBody)
            .Build();
                    
         await emailSender.SendAsync(message, cancellationToken);
         
        return Unit.Value;
    }
}