using Shared.Exceptions;
using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Exceptions;

namespace VocaBuddy.UI.Pages.Authentication;

public class RegisterBase : CustomComponentBase
{
    protected UserRegistrationRequestWithPasswordCheck Model = new();

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    [Inject]
    public ILogger<RegisterBase> Logger { get; set; }

    protected async Task ExecuteLogin()
    {
        IsLoading = true;

        try
        {
            await RegisterUserAsync();
            HandleSuccess("Successful registration.");
            await DisplayStatusMessageAsync();
            await SignInUserAsync();
            NavManager.NavigateTo("/");
        }
        catch (UserExistsException ex)
        {
            HandleError(ex);
        }
        catch (InvalidUserRegistrationInputException ex)
        {
            HandleError(ex);
        }
        catch (Exception ex)
        {
            HandleError(ex, "Registration failed.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private Task RegisterUserAsync()
        => AuthService.RegisterAsync(Model);

    private async Task SignInUserAsync()
    {
        var loginRequest = new UserLoginRequest()
        {
            Email = Model.Email,
            Password = Model.Password
        };

        await AuthService.LoginAsync(loginRequest);
    }
}
