using VocaBuddy.Shared.Exceptions;
using VocaBuddy.UI.Exceptions;

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
            HandleResult(result);
        }
        catch (InvalidCredentialsException ex)
        {
            ShowErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Login failed.");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private void HandleResult(IdentityResult result)
    {
        switch (result.Status)
        {
            case IdentityResultStatus.Success:
                NavManager.NavigateTo("/");
                break;
            case IdentityResultStatus.InvalidCredentials:
                throw new InvalidCredentialsException();
            default:
                throw new LoginFailedException(result.ErrorMessage!);
        }
    }

    private void ShowErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }
}
