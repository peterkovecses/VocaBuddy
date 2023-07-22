
using VocaBuddy.Shared.Exceptions;

namespace VocaBuddy.UI.Pages;

public partial class LoginBase : CustomComponentBase
{
    protected UserLoginRequest Model = new();
    protected bool ShowAuthError = false;
    protected string AuthErrorText = string.Empty;
    protected bool IsSubmitting { get; set; }

    [Inject]
    private IAuthenticationService _authService { get; set; }

    protected async Task ExecuteLogin()
    {
        IsSubmitting = true;
        ShowAuthError = false;

        try
        {
            var result = await _authService.LoginAsync(Model);
            NavManager.NavigateTo("/");
        }
        catch(InvalidCredentialsException ex)
        {
            ShowErrorMessage(ex.Message);
        }
        catch
        {
            ShowErrorMessage("Unsuccessful login");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private void ShowErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }
}
