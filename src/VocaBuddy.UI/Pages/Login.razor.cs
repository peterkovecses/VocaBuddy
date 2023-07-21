using Microsoft.AspNetCore.Components;
using VocaBuddy.UI.Exceptions;
using VocaBuddy.UI.Interfaces;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Pages;

public partial class LoginBase : CustomComponentBase
{
    protected UserLoginRequest Model = new();
    protected bool ShowAuthError = false;
    protected string AuthErrorText = string.Empty;

    [Inject]
    private IAuthenticationService _authService { get; set; }

    protected async Task ExecuteLogin()
    {
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
    }

    private void ShowErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }
}
