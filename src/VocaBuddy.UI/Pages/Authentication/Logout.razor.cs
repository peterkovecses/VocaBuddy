namespace VocaBuddy.UI.Pages.Authentication;

public class LogoutBase : CustomComponentBase
{
    [Inject]
    public IAuthenticationService AuthService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await AuthService.LogoutAsync();
        NavManager.NavigateTo("/");
    }
}
