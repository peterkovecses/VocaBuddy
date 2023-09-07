using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VocaBuddy.UI.ApiHelpers;
using VocaBuddy.UI.Authentication;
using VocaBuddy.UI.Services;

namespace VocaBuddy.UI;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConfigSection = configuration.GetSection(ConfigKeys.IdentityConfiguration);
        services.Configure<IdentityApiConfiguration>(identityConfigSection);
        var identityConfig = identityConfigSection.Get<IdentityApiConfiguration>();

        var vocaBuddyConfigSection = configuration.GetSection(ConfigKeys.VocaBuddyApiConfiguration);
        services.Configure<VocabuddyApiConfiguration>(vocaBuddyConfigSection);
        var vocaBuddyApiConfig = vocaBuddyConfigSection.Get<VocabuddyApiConfiguration>();

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
        services.AddScoped<IWordService, WordService>();
        services.AddSingleton<NotificationService>();

        services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri(identityConfig.BaseUrl);
        });

        services.AddHttpClient<IVocaBuddyApiClient, VocaBuddyApiClient>(client =>
        {
            client.BaseAddress = new Uri(vocaBuddyApiConfig.BaseUrl);
        });

        return services;
    }
}
