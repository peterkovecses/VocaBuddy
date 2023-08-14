namespace VocaBuddy.UI.Models;

public class Notification
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public bool AutoHide { get; set; }
}
