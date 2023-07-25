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
        services.Configure<IdentityApiConfiguration>(identityOptionsSection);
        var identityOptions = identityOptionsSection.Get<IdentityApiConfiguration>();

        services.Configure<PasswordConfiguration>(configuration.GetSection(ConfigKeys.PasswordOptions));

        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();

        services.AddScoped<
            CustomAuthenticationStateProvider,
              CustomAuthenticationStateProvider>();
                  services.AddScoped<AuthenticationStateProvider>(
                    provider => provider.GetService<CustomAuthenticationStateProvider>());

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri(identityOptions.BaseUrl);
        });

        return services;
    }
}
