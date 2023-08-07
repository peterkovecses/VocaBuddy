using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Exceptions;

namespace VocaBuddy.UI.Pages.Authentication;

public class LoginBase : NavComponentBase
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

    private void HandleResult(Result<TokenHolder> result)
    {
        if (result.IsSuccess)
        {
            NavManager.NavigateTo("/");
        }

        throw result.Error!.Code switch
        {
            IdentityErrorCode.InvalidCredentials => new InvalidCredentialsException(),
            _ => new LoginFailedException(result.Error!.Message),
        };
    }

    private void ShowErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }
}
