namespace Identity.Domain.Contracts;

public interface IEntity
{
    void AddDomainEvent(IDomainEvent domainDomainEvent);
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}