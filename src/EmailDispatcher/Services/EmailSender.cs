namespace EmailDispatcher.Services;

public class EmailSender(ILogger<EmailSender> logger, IOptions<SmtpSettings> smtpSettings) : IEmailSender, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly SmtpSettings _settings = smtpSettings.Value;
    private readonly MailKit.Net.Smtp.SmtpClient _smtpClient = new();
    private readonly TimeSpan _disconnectDelay = TimeSpan.FromSeconds(smtpSettings.Value.DisconnectDelaySeconds);
    private CancellationTokenSource? _disconnectCts;

    public async Task SendAsync(MimeMessage email, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to send email.");
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            await ConnectAsync(cancellationToken);
            await _smtpClient.SendAsync(email, cancellationToken);
            logger.LogInformation("Successfully sent email.");
               
                
            ScheduleDisconnect();
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private async Task ConnectAsync(CancellationToken cancellationToken)
    {
        if (!_smtpClient.IsConnected)
        {
            logger.LogInformation("Connecting to SMTP server...");
            await _smtpClient.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
            // Authentication can be added here if needed e.g. await _smtpClient.AuthenticateAsync("username", "password", cancellationToken);
            logger.LogInformation("Successfully connected to SMTP server.");
            return;
        }
        logger.LogInformation("Reusing existing SMTP connection.");
        await _smtpClient.NoOpAsync(cancellationToken);
    }

    private void ScheduleDisconnect()
    {
        logger.LogInformation("Scheduling disconnect in {Seconds} seconds.", _disconnectDelay.TotalSeconds);
        
        // Cancel any previously scheduled disconnect
        _disconnectCts?.Cancel();
        _disconnectCts?.Dispose();

        // Schedule a new disconnect
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        DelayedDisconnectAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private async Task DelayedDisconnectAsync( )
    {
        _disconnectCts = new CancellationTokenSource();
        var cancellationToken = _disconnectCts.Token;
        try
        {
            await Task.Delay(_disconnectDelay, cancellationToken);
            await DisconnectAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // This is expected if a new email is sent before the delay expires
            logger.LogInformation("Scheduled disconnect was canceled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred in the disconnect scheduler task.");
        }
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return;
        
        await _semaphore.WaitAsync(cancellationToken);
        
        try
        {
            if (!_smtpClient.IsConnected) return;
            logger.LogInformation("Attempting to disconnect from SMTP server...");
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            logger.LogInformation("Successfully disconnected from SMTP server.");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        logger.LogInformation("Disposing EmailSender...");
        if (_disconnectCts is not null) await _disconnectCts.CancelAsync();
        _disconnectCts?.Dispose();
        await DisconnectAsync(CancellationToken.None);
        _smtpClient.Dispose();
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}