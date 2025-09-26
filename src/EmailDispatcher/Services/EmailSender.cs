namespace EmailDispatcher.Services;

public class EmailSender(ILogger<EmailSender> logger) : IEmailSender
{
    public async Task SendAsync(MimeMessage email)
    {
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        await smtp.ConnectAsync("localhost", 1025);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
        logger.LogInformation("Email sent with Id {MessageId}", email.MessageId);
    }
}