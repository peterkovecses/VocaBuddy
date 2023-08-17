using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Services;

namespace VocaBuddy.UI.Shared;

public class LoginComponentBase : CustomComponentBase
{
    private const string LoginFailed = "Login failed.";

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    protected UserLoginRequest Model { get; set; } = new();

    protected async Task ExecuteLoginAsync()
    {
        try
        {
            Loading = true;
            var result = await AuthService.LoginAsync(Model);
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
        if (result.IsError)
        {
            StatusMessage = result.Error!.Code switch
            {
                IdentityErrorCode.InvalidCredentials => result.Error.Message,
                _ => LoginFailed
            };
        }
        else
        {
            NotificationService.ClearNotifications();
        }
    }
}
