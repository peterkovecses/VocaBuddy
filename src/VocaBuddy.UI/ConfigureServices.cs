using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VocaBuddy.UI.ApiHelper;
using VocaBuddy.UI.Authentication;

namespace VocaBuddy.UI;

public static class ConfigureServices
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConfigSection = configuration.GetSection(ConfigKeys.IdentityConfiguration);
        services.Configure<IdentityApiConfiguration>(identityConfigSection);
        var identityConfig = identityConfigSection.Get<IdentityApiConfiguration>();

        services.Configure<PasswordConfiguration>(configuration.GetSection(ConfigKeys.PasswordConfiguration));

        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();

        services.AddScoped<
            CustomAuthenticationStateProvider,
              CustomAuthenticationStateProvider>();
                  services.AddScoped<AuthenticationStateProvider>(
                    provider => provider.GetService<CustomAuthenticationStateProvider>());

        services.AddScoped<IJwtParser, JwtParser>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri(identityConfig.BaseUrl);
        });

        return services;
    }
}
