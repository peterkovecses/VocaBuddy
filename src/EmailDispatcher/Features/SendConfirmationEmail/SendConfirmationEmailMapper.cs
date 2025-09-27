namespace EmailDispatcher.Features.SendConfirmationEmail;

public static class SendConfirmationEmailMapper
{
    public static SendConfirmationEmailCommand ToSendEmailCommand(this UserRegistered eventData) =>
        new()
        {
            EventId = eventData.EventId,
            UserId = eventData.UserId,
            Email = eventData.Email,
            FirstName = eventData.FirstName,
            LastName = eventData.LastName,
            ConfirmationLink = eventData.ConfirmationLink
        };
}