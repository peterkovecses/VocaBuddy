namespace VocaBuddy.UI.Shared.BaseComponents;

public class CustomComponentBase : ComponentBase, IDisposable
{
    private CancellationTokenSource? _cancellationTokenSource;
    protected string? StatusMessage { get; set; }
    protected bool OperationSucceeded { get; set; }
    protected bool Loading { get; set; }

    [Inject]
    protected NavigationManager? NavManager { get; set; }

    [Inject]
    public NotificationService? NotificationService { get; set; }
    
    protected CancellationToken CancellationToken 
        => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;

    protected bool IsStatusMessageSet
        => !string.IsNullOrEmpty(StatusMessage);

    protected void ClearStatusMessage()
        => StatusMessage = string.Empty;

    protected void SessionExpired()
    {
        NotificationService!.ShowFailure("The session has expired, please log in again.");
        NavManager!.NavigateTo("/logout");
    }

    public virtual void Dispose()
    {
        if (_cancellationTokenSource is null) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }
}
