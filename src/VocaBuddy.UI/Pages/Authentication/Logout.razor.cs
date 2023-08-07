﻿using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Authentication;

public class LogoutBase : NavComponentBase
{
    [Inject]
    public IAuthenticationService AuthService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await AuthService.LogoutAsync();
        NavManager.NavigateTo("/");
    }
}