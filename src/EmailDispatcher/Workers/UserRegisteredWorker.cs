using VocaBuddy.Shared.DomainEvents.Identity;

namespace EmailDispatcher.Workers;

public class UserRegisteredWorker(IBus bus, IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<UserRegisteredDomainEvent>("email-dispatcher-user-registered",
            async data =>
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(data.ToSendEmailCommand(), stoppingToken);
            },
            cancellationToken: stoppingToken);
    }
}