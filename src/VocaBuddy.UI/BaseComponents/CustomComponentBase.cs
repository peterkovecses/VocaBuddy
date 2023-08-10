namespace VocaBuddy.UI.BaseComponents;

public class CustomComponentBase : ComponentBase
{
    protected string StatusMessage { get; set; }
    protected bool OperationSucceeded { get; set; }
    protected bool IsLoading { get; set; }

    [Inject]
    protected NavigationManager NavManager { get; set; }

    protected bool IsStatusMessageSet
        => !string.IsNullOrEmpty(StatusMessage);

    protected async Task DisplaySuccess(string message)
    {
        StatusMessage = message;
        OperationSucceeded = true;
        StateHasChanged();
        await Task.Delay(1500);
    }
}
