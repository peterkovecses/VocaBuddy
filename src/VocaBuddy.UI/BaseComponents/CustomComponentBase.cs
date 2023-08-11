namespace VocaBuddy.UI.BaseComponents;

public class CustomComponentBase : ComponentBase
{
    protected string StatusMessage { get; set; }
    protected bool OperationSucceeded { get; set; }
    protected bool Loading { get; set; }

    [Inject]
    protected NavigationManager NavManager { get; set; }

    protected bool IsStatusMessageSet
        => !string.IsNullOrEmpty(StatusMessage);

    protected async Task DisplaySuccessAsync(string message)
    {
        StatusMessage = message;
        OperationSucceeded = true;
        StateHasChanged();
        await Task.Delay(1500);
    }

    protected void ClearStatusMessage()
        => StatusMessage = string.Empty;
}
