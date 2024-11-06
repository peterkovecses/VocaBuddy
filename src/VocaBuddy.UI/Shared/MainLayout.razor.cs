namespace VocaBuddy.UI.Shared;

public class MainLayoutBase : LayoutComponentBase, IDisposable
{
    protected ErrorBoundary? ErrorBoundary;
    protected bool IsAuthenticated;
    protected string? Email;        

    [CascadingParameter]
    protected Task<AuthenticationState>? AuthState { get; set; }

    [Inject]
    protected AuthenticationStateProvider? AuthStateProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        (AuthStateProvider! as AuthenticationStateProvider).AuthenticationStateChanged += OnAuthenticationStateChanged;
        await base.OnInitializedAsync();
        await UpdateAuthenticationState();
    }

    protected override void OnParametersSet()
    {
       ErrorBoundary?.Recover();
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        => await UpdateAuthenticationState();

    private async Task UpdateAuthenticationState()
    {
        var authState = await AuthState!;

        if (authState.User.Identity!.IsAuthenticated)
        {
            IsAuthenticated = true;
            Email = authState.User.Claims.Single(claim => claim.Type == "email").Value;
        }
        else
        {
            IsAuthenticated = false;
            Email = "";
        }

        StateHasChanged();
    }

    public void Dispose()
        => AuthStateProvider!.AuthenticationStateChanged -= OnAuthenticationStateChanged;
}