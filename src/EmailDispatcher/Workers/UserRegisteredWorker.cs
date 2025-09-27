namespace EmailDispatcher.Workers;

public class UserRegisteredWorker(ILogger<UserRegisteredWorker> logger, IBus bus, IMediator mediator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<UserRegistered>("email-dispatcher-user-registered",
            async data =>
            {
                logger.LogInformation("Received UserRegistered event with Id: {EventId}", data.EventId);
                await mediator.Send(data.ToSendEmailCommand(), stoppingToken);
            },
            cancellationToken: stoppingToken);
    }
}