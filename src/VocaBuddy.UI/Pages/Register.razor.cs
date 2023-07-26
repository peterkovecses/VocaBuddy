using Shared.Exceptions;

namespace VocaBuddy.UI.Pages;

public partial class RegisterBase : CustomComponentBase
{
    protected UserRegistrationRequest Model = new();
    protected bool ShowAuthError = false;
    protected string AuthErrorText = string.Empty;
    protected bool ShowSuccessMessage = false;
    protected string SuccessMessageText = "Successful registration";
    protected bool IsSubmitting { get; set; }

    [Inject]
    private IAuthenticationService _authService { get; set; }

    protected async Task ExecuteLogin()
    {
        IsSubmitting = true;
        ShowAuthError = false;

        try
        {
            await RegisterUser();
            await SignInUser();
            await DisplaySuccessMessage();
            NavigateToIndexPage();
        }
        catch (UserCreationException ex)
        {
            DisplayErrorMessage(ex.Message);
        }
        catch
        {
            DisplayErrorMessage("Something went wrong.");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private async Task RegisterUser()
    {
        await _authService.RegisterAsync(Model);
    }

    private async Task SignInUser()
    {
        var loginRequest = new UserLoginRequest()
        {
            Email = Model.Email,
            Password = Model.Password
        };

        await _authService.LoginAsync(loginRequest);
    }

    private async Task DisplaySuccessMessage()
    {
        ShowSuccessMessage = true;
        StateHasChanged();
        await Task.Delay(1500);
    }

    private void DisplayErrorMessage(string message)
    {
        AuthErrorText = message;
        ShowAuthError = true;
    }

    private void NavigateToIndexPage()
    {
        NavManager.NavigateTo("/");
    }
}
