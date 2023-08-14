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

    protected void ClearStatusMessage()
        => StatusMessage = string.Empty;
}
