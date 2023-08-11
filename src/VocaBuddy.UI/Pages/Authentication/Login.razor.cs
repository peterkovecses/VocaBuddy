using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Authentication;

public class LoginBase : CustomComponentBase
{
    protected UserLoginRequest Model = new();

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    protected async Task ExecuteLogin()
    {
        Loading = true;
        var result = await AuthService.LoginAsync(Model);
        Loading = false;
        HandleResult(result);
    }

    private void HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            NavManager.NavigateTo("/");
        }
        else
        {
            StatusMessage = result.Error!.Code switch
            {
                IdentityErrorCode.InvalidCredentials => result.Error.Message,
                _ => "Login failed."
            };
        }
    }
}
