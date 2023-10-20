namespace VocaBuddy.UI.Services;

public class NotificationService
{
    public event Action<Notification> OnNotificationAdded;
    public event Action OnNotificationsCleared;

    public void ShowNotification(string message, bool isSuccess, bool autoHide)
    {
        OnNotificationAdded?.Invoke(new Notification
        {
            Id = Guid.NewGuid(),
            Message = message,
            IsSuccess = isSuccess,
            AutoHide = autoHide
        });
    }

    public void ShowSuccess(string message)
        => ShowNotification(message, true, true);

    public void ShowFailure(string message = "Something went wrong, please try again later.")
        => ShowNotification(message, false, false);

    public void ClearNotifications()
        => OnNotificationsCleared?.Invoke();
}
