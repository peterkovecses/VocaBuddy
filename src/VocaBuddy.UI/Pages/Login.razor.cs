using VocaBuddy.Shared.Exceptions;
using VocaBuddy.UI.Exceptions;

namespace VocaBuddy.UI.Pages;

public class LoginBase : CustomComponentBase
{
    protected UserLoginRequest Model = new();
    protected bool ShowAuthError = false;
    protected string AuthErrorText = string.Empty;
    protected bool IsSubmitting { get; set; }

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    [Inject]
    public ILogger<LoginBase> Logger { get; set; }

    protected async Task ExecuteLogin()
    {
        IsSubmitting = true;
        ShowAuthError = false;

        try
        {
            var result = await AuthService.LoginAsync(Model);
            HandleResult(result);
        }
        catch (InvalidCredentialsException ex)
        {
            Logger.LogError(ex, "Invalid credentials exception occured");
            ShowErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occured");
            ShowErrorMessage("Login failed.");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private void HandleResult(IdentityResult result)
    {
        if (result.IsSuccess)
        {
            NavManager.NavigateTo("/");
        }

        throw result.Error switch
        {
            IdentityError.InvalidCredentials => new InvalidCredentialsException(),
            _ => new LoginFailedException(result.ErrorMessage!),
        };
    }

    private void ShowErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }
}
