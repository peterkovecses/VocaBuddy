using static Microsoft.EntityFrameworkCore.EntityState;

namespace VocaBuddy.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    private void UpdateNativeWordTimestamps(DbContextEventData eventData)
    {
        foreach (var wordEntityEntry in eventData.Context!.ChangeTracker.Entries<NativeWord>())
        {
            if (wordEntityEntry.State is Added)
            {
                wordEntityEntry.Entity.CreatedUtc = _dateTimeProvider.UtcNow;
            }

            if (wordEntityEntry.State is Added or Modified)
            {
                wordEntityEntry.Entity.UpdatedUtc = _dateTimeProvider.UtcNow;
            }
        }
    }

    public void UpdateNativeWordTimestampsOnTranslationUpdate(DbContextEventData eventData)
    {
        foreach (var wordEntityEntry in eventData.Context!.ChangeTracker.Entries<ForeignWord>())
        {
            if (wordEntityEntry.State is Modified)
            {
                var nativeWord = eventData.Context.Set<NativeWord>().Single(nativeWord => nativeWord.Id == wordEntityEntry.Entity.NativeWordId);
                nativeWord.UpdatedUtc = _dateTimeProvider.UtcNow;
            }
        }
    }

    public async Task UpdateNativeWordTimestampsOnTranslationUpdateAsync(DbContextEventData eventData)
    {
        foreach (var wordEntityEntry in eventData.Context!.ChangeTracker.Entries<ForeignWord>())
        {
            if (wordEntityEntry.State is Modified)
            {
                var nativeWord = await eventData.Context.Set<NativeWord>().SingleAsync(nativeWord => nativeWord.Id == wordEntityEntry.Entity.NativeWordId);
                nativeWord.UpdatedUtc = _dateTimeProvider.UtcNow;
            }
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateNativeWordTimestamps(eventData);
        UpdateNativeWordTimestampsOnTranslationUpdate(eventData);

        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateNativeWordTimestamps(eventData);
        await UpdateNativeWordTimestampsOnTranslationUpdateAsync(eventData);        

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
