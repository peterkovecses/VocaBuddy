using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VocaBuddy.UI.ApiHelper.IdentityApi;
using VocaBuddy.UI.Authentication.Models;
using VocaBuddy.UI.Authentication.Services;

namespace VocaBuddy.UI;

public static class ConfigureServices
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var identityOptionsSection = configuration.GetSection(ConfigKeys.IdentityOptions);
        services.Configure<IdentityApiOptions>(identityOptionsSection);           
        var identityOptions = identityOptionsSection.Get<IdentityApiOptions>();

        services.Configure<PasswordOptions>(configuration.GetSection(ConfigKeys.PasswordOptions));

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
