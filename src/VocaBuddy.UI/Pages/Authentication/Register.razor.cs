using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;
using VocaBuddy.UI.Services;

namespace VocaBuddy.UI.Pages.Authentication;

public class RegisterBase : CustomComponentBase
{
    private const string RegistrationFailed = "Registration failed.";

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    protected UserRegistrationRequestWithPasswordCheck Model { get; set; } = new();

    protected async Task ExecuteRegistrationAsync()
    {
        try
        {
            Loading = true;
            var result = await AuthService.RegisterAsync(Model);
            await HandleResultAsync(result);
        }
        catch
        {
            StatusMessage = RegistrationFailed;
        }
        finally
        {
            Loading = false;
        }
    }

    private async Task HandleResultAsync(Result result)
    {
        if (result.IsSuccess)
        {
            NotificationService.ShowSuccess("Successful registration.");
            await SignInUserAsync();
            NavManager.NavigateTo("/");
        }

        StatusMessage = result.Error!.Code switch
        {
            IdentityErrorCode.UserExists => result.Error.Message,
            IdentityErrorCode.InvalidUserRegistrationInput => result.Error.Message,
            _ => RegistrationFailed
        };
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
}
