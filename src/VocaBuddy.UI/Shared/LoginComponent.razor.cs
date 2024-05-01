namespace VocaBuddy.UI.Shared;

public class LoginComponentBase : CustomComponentBase
{
    public const string LoginFailed = "Login failed.";

    [Inject]
    public IAuthenticationService? AuthService { get; set; }

    protected UserLoginRequest Model { get; set; } = new();

    protected async Task ExecuteLoginAsync()
    {
        try
        {
            Loading = true;
            var result = await AuthService!.LoginAsync(Model);
            HandleResult(result);
        }
        catch
        {
            StatusMessage = LoginFailed;
        }
        finally
        {
            Loading = false;
        }
    }

    private void HandleResult(Result result)
    {
        if (result.IsFailure)
        {
            StatusMessage = result.ErrorInfo!.Code switch
            {
                IdentityErrorCode.InvalidCredentials => result.ErrorInfo.Errors.First().Message,
                _ => LoginFailed
            };
        }
        else
        {
            NotificationService!.ClearNotifications();
        }
    }
}
