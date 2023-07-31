using Shared.Exceptions;
using VocaBuddy.UI.Exceptions;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Pages;

public partial class RegisterBase : CustomComponentBase
{
    protected UserRegistrationRequestWithPasswordCheck Model = new();
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
            var result = await RegisterUserAsync();
            ValidateResult(result);
            await SignInUserAsync();
            await DisplaySuccessMessageAsync();
            NavigateToIndexPage();
        }
        catch (UserExistsException ex)
        {
            DisplayErrorMessage(ex.Message);
        }
        catch (InvalidUserRegistrationInputException ex)
        {
            DisplayErrorMessage(ex.Message);
        }
        catch
        {
            DisplayErrorMessage("Registration failed.");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private Task<IdentityResult> RegisterUserAsync()
        => _authService.RegisterAsync(Model);

    private static void ValidateResult(IdentityResult result)
    {
        if (result.IsError)
        {
            throw result.Error switch
            {
                IdentityError.UserExists => new UserExistsException(),
                IdentityError.InvalidCredentials => new InvalidUserRegistrationInputException(result.ErrorMessage!),
                _ => new RegistrationFailedException(result.ErrorMessage!),
            };
        }
    }

    private async Task SignInUserAsync()
    {
        var loginRequest = new UserLoginRequest()
        {
            Email = Model.Email,
            Password = Model.Password
        };

        await _authService.LoginAsync(loginRequest);
    }

    private async Task DisplaySuccessMessageAsync()
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
