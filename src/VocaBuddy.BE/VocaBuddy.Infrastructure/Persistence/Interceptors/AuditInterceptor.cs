using static Microsoft.EntityFrameworkCore.EntityState;

namespace VocaBuddy.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor(IDateTimeProvider dateTimeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntityTimestamps(eventData);
        UpdateNativeWordTimestampsOnTranslationUpdate(eventData);

        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntityTimestamps(eventData);
        await UpdateNativeWordTimestampsOnTranslationUpdateAsync(eventData);        

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private void UpdateEntityTimestamps(DbContextEventData eventData)
    {
        foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State is Added)
            {
                entityEntry.Entity.CreatedUtc = dateTimeProvider.UtcNow;
            }

            if (entityEntry.State is Added or Modified)
            {
                entityEntry.Entity.UpdatedUtc = dateTimeProvider.UtcNow;
            }
        }
    }

    private void UpdateNativeWordTimestampsOnTranslationUpdate(DbContextEventData eventData)
    {
        foreach (var foreignWordEntityEntry in eventData.Context!.ChangeTracker.Entries<ForeignWord>())
        {
            if (foreignWordEntityEntry.State is not (Added or Modified)) continue;
            var nativeWordId = foreignWordEntityEntry.Entity.NativeWordId;
            if (nativeWordId == 0) continue;
            var nativeWord = eventData.Context.Set<NativeWord>().Single(nativeWord => nativeWord.Id == nativeWordId);
            nativeWord.UpdatedUtc = dateTimeProvider.UtcNow;
        }
    }

    private async Task UpdateNativeWordTimestampsOnTranslationUpdateAsync(DbContextEventData eventData)
    {
        foreach (var foreignWordEntityEntry in eventData.Context!.ChangeTracker.Entries<ForeignWord>())
        {
            if (foreignWordEntityEntry.State is not (Added or Modified)) continue;
            var nativeWordId = foreignWordEntityEntry.Entity.NativeWordId;
            if (nativeWordId == 0) continue;
            var nativeWord = await eventData.Context.Set<NativeWord>().SingleAsync(nativeWord => nativeWord.Id == nativeWordId);
            nativeWord.UpdatedUtc = dateTimeProvider.UtcNow;
        }
    }
}