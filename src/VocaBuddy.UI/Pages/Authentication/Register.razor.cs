using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Authentication;

public class RegisterBase : CustomComponentBase
{
    protected UserRegistrationRequestWithPasswordCheck Model = new();

    [Inject]
    public IAuthenticationService AuthService { get; set; }

    protected async Task ExecuteLogin()
    {
        IsLoading = true;
        var result = await AuthService.RegisterAsync(Model);
        IsLoading = false;
        await HandleResult(result);
    }

    private async Task HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            await DisplaySuccess("Successful registration.");
            await SignInUserAsync();
            NavManager.NavigateTo("/");
        }
        else
        {
            StatusMessage = result.Error!.Code switch
            {
                IdentityErrorCode.UserExists => result.Error.Message,
                IdentityErrorCode.InvalidUserRegistrationInput => result.Error.Message,
                _ => "Registration failed."
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
}
