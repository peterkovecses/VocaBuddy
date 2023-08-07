using Shared.Exceptions;
using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Exceptions;

namespace VocaBuddy.UI.Pages.Authentication;

public class RegisterBase : NavComponentBase
{
    protected UserRegistrationRequestWithPasswordCheck Model = new();
    protected bool ShowAuthError = false;
    protected string AuthErrorText = string.Empty;
    protected bool ShowSuccessMessage = false;
    protected string SuccessMessageText = "Successful registration";
    protected bool IsSubmitting { get; set; }

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    [Inject]
    public ILogger<RegisterBase> Logger { get; set; }

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
            Logger.LogError(ex, "User exists exception occured");
            DisplayErrorMessage(ex.Message);
        }
        catch (InvalidUserRegistrationInputException ex)
        {
            Logger.LogError(ex, "Invalid user registration input exception occured");
            DisplayErrorMessage(ex.Message);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "An exception occured");
            DisplayErrorMessage("Registration failed.");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private Task<Result<ErrorInfo>> RegisterUserAsync()
        => AuthService.RegisterAsync(Model);

    private static void ValidateResult(Result<ErrorInfo> result)
    {
        if (result.IsError)
        {
            throw result.Error!.Code switch
            {
                IdentityErrorCode.UserExists => new UserExistsException(),
                IdentityErrorCode.InvalidUserRegistrationInput => new InvalidUserRegistrationInputException(result.Error!.Message),
                _ => new RegistrationFailedException(result.Error!.Message),
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

        await AuthService.LoginAsync(loginRequest);
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
