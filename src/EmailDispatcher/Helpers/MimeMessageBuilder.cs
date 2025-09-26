namespace EmailDispatcher.Helpers;

public class MimeMessageBuilder
{
    private readonly MimeMessage _message;

    private MimeMessageBuilder()
    {
        _message = new MimeMessage();
    }
    
    public MimeMessageBuilder WithMessageId(string messageId)
    {
        _message.MessageId = messageId;
        return this;
    }
    
    public static MimeMessageBuilder StartBuilding() => new();

    public MimeMessageBuilder WithSender(string name, string email)
    {
        _message.From.Add(new MailboxAddress(name, email));
        return this;
    }

    public MimeMessageBuilder WithRecipient(string name, string email)
    {
        _message.To.Add(new MailboxAddress(name, email));
        return this;
    }

    public MimeMessageBuilder WithSubject(string subject)
    {
        _message.Subject = subject;
        return this;
    }

    public MimeMessageBuilder WithBody(string textBody)
    {
        var bodyBuilder = new BodyBuilder
        {
            TextBody = textBody
        };
        _message.Body = bodyBuilder.ToMessageBody();
        return this;
    }

    public MimeMessage Build() => _message;
}
