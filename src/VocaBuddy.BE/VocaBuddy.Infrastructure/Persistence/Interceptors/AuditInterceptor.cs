using Microsoft.EntityFrameworkCore.Diagnostics;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Infrastructure.Interfaces;
using static Microsoft.EntityFrameworkCore.EntityState;

namespace VocaBuddy.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    private void SetWordCreationTime(DbContextEventData eventData)
    {
        foreach (var wordEntityEntry in eventData.Context.ChangeTracker.Entries<NativeWord>())
        {
            if (wordEntityEntry.State == Added)
            {
                wordEntityEntry.Entity.CreatedUtc = _dateTimeProvider.UtcNow;
            }
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetWordCreationTime(eventData);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetWordCreationTime(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
