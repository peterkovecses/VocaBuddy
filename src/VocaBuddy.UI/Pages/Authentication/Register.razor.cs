namespace VocaBuddy.UI.Pages.Authentication;

public class RegisterBase : CustomComponentBase
{
    private const string RegistrationFailed = "Registration failed.";

    [Inject]
    public IAuthenticationService? AuthService { get; set; }

    protected UserRegistrationRequestWithPasswordCheck Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (await AuthService!.IsUserAuthenticatedAsync())
        {
            NavManager!.NavigateTo("/");
        }
    }

    protected async Task ExecuteRegistrationAsync()
    {
        try
        {
            Loading = true;
            var result = await AuthService!.RegisterAsync(Model, CancellationToken);
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
            IdentityErrorCodes.UserExists => result.ErrorInfo.Errors.First().Message,
            IdentityErrorCodes.InvalidUserRegistrationInput => result.ErrorInfo.Errors.First().Message,
            _ => RegistrationFailed
        };
    }

    private async Task SignInUserAsync()
    {
        var loginRequest = new UserLoginRequest
        {
            Email = Model.Email,
            Password = Model.Password
        };

        await AuthService!.LoginAsync(loginRequest, CancellationToken);
    }
}
