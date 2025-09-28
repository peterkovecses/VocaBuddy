namespace EmailDispatcher.Workers;

public class UserRegisteredWorker(Serilog.ILogger logger, IBus bus, IMediator mediator)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<UserRegistered>("email-dispatcher-user-registered",
            BusHandlerWrapper.Handle<UserRegistered>(async data => await mediator.Send(data.ToSendEmailCommand(), stoppingToken), logger),
            cancellationToken: stoppingToken);
    }
}