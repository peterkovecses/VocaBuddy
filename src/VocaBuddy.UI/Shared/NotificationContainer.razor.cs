using VocaBuddy.UI.Services;

namespace VocaBuddy.UI.Shared;

public class NotificationContainerBase : ComponentBase
{
    [Inject]
    public NotificationService NotificationService { get; set; }

    protected List<Notification> Notifications = new();

    protected override void OnInitialized()
    {
        NotificationService.OnNotificationAdded += AddNotification;
    }

    protected void RemoveNotification(Notification notification)
    {
        Notifications.Remove(notification);
        StateHasChanged();
    }

    private void AddNotification(Notification notification)
    {
        Notifications.Add(notification);
        StateHasChanged();

        if (notification.AutoHide)
        {
            _ = AutoHideNotificationAsync(notification);
        }
    }

    private async Task AutoHideNotificationAsync(Notification notification)
    {
        await Task.Delay(5000);
        RemoveNotification(notification);
    }
}
