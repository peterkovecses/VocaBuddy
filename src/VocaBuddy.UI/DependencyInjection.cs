using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;

namespace VocaBuddy.UI;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConfigSection = configuration.GetSection(ConfigKeys.IdentityConfiguration);
        services.Configure<IdentityApiConfiguration>(identityConfigSection);
        var identityConfig = identityConfigSection.Get<IdentityApiConfiguration>();

        var vocaBuddyConfigSection = configuration.GetSection(ConfigKeys.VocaBuddyApiConfiguration);
        services.Configure<VocaBuddyApiConfiguration>(vocaBuddyConfigSection);
        var vocaBuddyApiConfig = vocaBuddyConfigSection.Get<VocaBuddyApiConfiguration>();

        services.Configure<PasswordConfiguration>(configuration.GetSection(ConfigKeys.PasswordConfiguration));

        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();

        services.AddScoped<
            CustomAuthenticationStateProvider,
              CustomAuthenticationStateProvider>();
                  services.AddScoped<AuthenticationStateProvider>(
                    provider => provider.GetService<CustomAuthenticationStateProvider>()!);

        services.AddValidatorsFromAssembly(UiAssemblyMarker.Assembly);

        services.AddScoped<IJwtParser, JwtParser>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IWordService, WordService>();
        services.AddTransient<IGamePlayService, GamePlayService>();
        services.AddSingleton<NotificationService>();

        services.AddHttpClient<IIdentityApiClient, IdentityApiClient>(client =>
        {
            client.BaseAddress = new Uri(identityConfig!.BaseUrl);
        });

        services.AddHttpClient<IVocaBuddyApiClient, VocaBuddyApiClient>(client =>
        {
            client.BaseAddress = new Uri(vocaBuddyApiConfig!.BaseUrl);
        });

        return services;
    }
}
