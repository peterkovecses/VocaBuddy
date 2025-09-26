namespace EmailDispatcher.Services;

public interface IEmailSender
{
    Task SendAsync(MimeMessage email);
}
