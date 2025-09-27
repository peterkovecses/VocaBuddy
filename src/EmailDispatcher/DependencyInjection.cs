namespace EmailDispatcher;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        services.AddEasyNetQ("host=localhost").UseSystemTextJson(serializerOptions);
        services.AddSingleton<IEmailSender, EmailSender>();
        services.AddHostedService<UserRegisteredWorker>();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyMarker.Assembly));

        return services;
    }
}