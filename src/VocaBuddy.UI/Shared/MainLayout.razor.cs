using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace VocaBuddy.UI.Shared;

public class MainLayoutBase : LayoutComponentBase
{
    protected bool IsAuthenticated;
    protected string Email;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState;

        if (authState.User.Identity.IsAuthenticated)
        {
            IsAuthenticated = true;
            Email = authState.User.Claims.Single(claim => claim.Type == "email").Value;
        }
    }
}
