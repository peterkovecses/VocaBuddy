using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VocaBuddy.UI.Api.IdentityApi;
using VocaBuddy.UI.Authentication;
using VocaBuddy.UI.Interfaces;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI;

public static class ConfigureServices
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var identityOptionsSection = configuration.GetSection(IdentityOptions.Identity);
        services.Configure<IdentityOptions>(identityOptionsSection);           
        var identityOptions = identityOptionsSection.Get<IdentityOptions>();

        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri(identityOptions.BaseUrl);
        });

        return services;
    }
}
