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
        foreach (var wordEntityEntry in eventData.Context!.ChangeTracker.Entries<EntityBase>())
        {
            if (wordEntityEntry.State is Added)
            {
                wordEntityEntry.Entity.CreatedUtc = dateTimeProvider.UtcNow;
            }

            if (wordEntityEntry.State is Added or Modified)
            {
                wordEntityEntry.Entity.UpdatedUtc = dateTimeProvider.UtcNow;
            }
        }
    }

    private void UpdateNativeWordTimestampsOnTranslationUpdate(DbContextEventData eventData)
    {
        foreach (var wordEntityEntry in eventData.Context!.ChangeTracker.Entries<ForeignWord>())
        {
            if (wordEntityEntry.State is not Modified) continue;
            var nativeWord = eventData.Context.Set<NativeWord>().Single(nativeWord => nativeWord.Id == wordEntityEntry.Entity.NativeWordId);
            nativeWord.UpdatedUtc = dateTimeProvider.UtcNow;
        }
    }

    private async Task UpdateNativeWordTimestampsOnTranslationUpdateAsync(DbContextEventData eventData)
    {
        foreach (var wordEntityEntry in eventData.Context!.ChangeTracker.Entries<ForeignWord>())
        {
            if (wordEntityEntry.State is not Modified) continue;
            var nativeWord = await eventData.Context.Set<NativeWord>().SingleAsync(nativeWord => nativeWord.Id == wordEntityEntry.Entity.NativeWordId);
            nativeWord.UpdatedUtc = dateTimeProvider.UtcNow;
        }
    }
}
