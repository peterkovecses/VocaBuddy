using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Authentication;

public class RegisterBase : CustomComponentBase
{
    public const string RegistrationFailed = "Registration failed.";

    [Inject]
    public IAuthenticationService? AuthService { get; set; }

    protected UserRegistrationRequestWithPasswordCheck Model { get; set; } = new();

    protected async Task ExecuteRegistrationAsync()
    {
        try
        {
            Loading = true;
            var result = await AuthService!.RegisterAsync(Model);
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
            NotificationService!.ShowSuccess("Successful registration.");
            await SignInUserAsync();
            NavManager!.NavigateTo("/");
        }

        StatusMessage = result.ErrorInfo!.Code switch
        {
            IdentityErrorCode.UserExists => result.ErrorInfo.Errors.First().Message,
            IdentityErrorCode.InvalidUserRegistrationInput => result.ErrorInfo.Errors.First().Message,
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

        await AuthService!.LoginAsync(loginRequest);
    }
}
