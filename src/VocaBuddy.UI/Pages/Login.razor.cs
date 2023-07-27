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

        var result = await _authService.LoginAsync(Model);
        HandleResult(result);
    }

    private void HandleResult(AuthenticationResult result)
    {
        IsSubmitting = false;

        switch (result.Status)
        {
            case AuthenticationResultStatus.Success:
                NavManager.NavigateTo("/");
                break;
            case AuthenticationResultStatus.InvalidCredentials:
                ShowErrorMessage(result.ErrorMessage!);
                break;
            default:
                ShowErrorMessage("Unsuccessful login");
                break;
        }
    }

    private void ShowErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }
}
