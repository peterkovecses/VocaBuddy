using VocaBuddy.Shared.DomainEvents.Identity;

namespace EmailDispatcher.Features.SendConfirmationEmail;

public static class SendConfirmationEmailMapper
{
    public static SendConfirmationEmailCommand ToSendEmailCommand(this UserRegisteredDomainEvent domainEventData) =>
        new()
        {
            EventId = domainEventData.EventId,
            Email = domainEventData.Email,
            FirstName = domainEventData.FirstName,
            LastName = domainEventData.LastName,
            ConfirmationLink = domainEventData.ConfirmationLink
        };
}