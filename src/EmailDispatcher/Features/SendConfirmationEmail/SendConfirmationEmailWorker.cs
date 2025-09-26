namespace EmailDispatcher.Features.SendConfirmationEmail;

public class SendConfirmationEmailWorker(ILogger<SendConfirmationEmailWorker> logger, IBus bus, IMediator mediator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<UserRegistered>("email-dispatcher-user-registered",
            async data =>
            {
                logger.LogInformation("Received UserRegistered event with Id: {MessageId}", data.Id);
                await mediator.Send(data.ToSendEmailCommand(), stoppingToken);
            },
            cancellationToken: stoppingToken);
    }
}