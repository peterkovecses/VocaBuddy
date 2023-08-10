using VocaBuddy.UI.Pages.Authentication;

namespace VocaBuddy.UI.BaseComponents;

public class CustomComponentBase : ComponentBase
{
    protected string StatusMessage { get; set; }
    protected bool OperationSucceeded { get; set; }
    protected bool IsLoading { get; set; }

    [Inject]
    protected NavigationManager NavManager { get; set; }

    [Inject]
    public ILogger<LoginBase> Logger { get; set; }

    protected bool IsStatusMessageSet
        => !string.IsNullOrEmpty(StatusMessage);

    protected async Task DisplaySuccess(string message)
    {
        StatusMessage = message;
        OperationSucceeded = true;
        StateHasChanged();
        await Task.Delay(1500);
    }

    protected void HandleError(Exception exception)
    {
        Logger.LogError(exception, "An exception occured");
        StatusMessage = exception.Message;
    }

    protected void HandleError(Exception exception, string message)
    {
        Logger.LogError(exception, "An exception occured");
        StatusMessage = message;
    }
}
