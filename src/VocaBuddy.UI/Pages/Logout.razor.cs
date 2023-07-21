using Microsoft.AspNetCore.Components;
using VocaBuddy.UI.Interfaces;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Pages;

public class LogoutBase : CustomComponentBase
{
    [Inject]
    private IAuthenticationService _authService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await _authService.LogoutAsync();
        NavManager.NavigateTo("/");
    }
}
