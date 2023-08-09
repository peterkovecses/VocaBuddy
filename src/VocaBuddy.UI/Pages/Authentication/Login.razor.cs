using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Exceptions;

namespace VocaBuddy.UI.Pages.Authentication;

public class LoginBase : CustomComponentBase
{
    protected UserLoginRequest Model = new();

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    [Inject]
    public ILogger<LoginBase> Logger { get; set; }

    protected async Task ExecuteLogin()
    {
        IsLoading = true;

        try
        {
            await AuthService.LoginAsync(Model);
            NavManager.NavigateTo("/");
        }
        catch (InvalidCredentialsException ex)
        {
            HandleError(ex);
        }
        catch (Exception ex)
        {
            HandleError(ex, "Login failed.");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
