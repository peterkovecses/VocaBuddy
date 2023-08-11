namespace VocaBuddy.UI.Shared;

public class DeleteConfirmationBase : ComponentBase
{
    [Parameter] 
    public bool IsModalVisible { get; set; }

    [Parameter] 
    public EventCallback OnConfirm { get; set; }

    [Parameter] 
    public EventCallback CloseModal { get; set; }
}
