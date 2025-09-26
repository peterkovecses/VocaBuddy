namespace EmailDispatcher.Features.SendConfirmationEmail;

public static class SendConfirmationEmailMapper
{
    public static SendConfirmationEmailCommand ToSendEmailCommand(this UserRegistered eventData) =>
        new()
        {
            Id = eventData.Id,
            Email = eventData.Email,
            FirstName = eventData.FirstName,
            LastName = eventData.LastName,
            ConfirmationLink = eventData.ConfirmationLink
        };
}