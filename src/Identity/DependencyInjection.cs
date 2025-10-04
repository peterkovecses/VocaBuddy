namespace Identity;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataProtection();

        services.AddDbContext<IdentityContext>((sp, options) =>
            options
                .UseSqlServer(configuration.GetConnectionString("IdentityDatabase"))
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));


        var tokenValidationParametersConfigSection = configuration.GetSection("TokenValidationParameters");
        var tokenValidationParameters = new CustomTokenValidationParameters();
        tokenValidationParametersConfigSection.Bind(tokenValidationParameters);
        services.Configure<CustomTokenValidationParameters>(tokenValidationParametersConfigSection);

        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        services.AddScoped<IIdentityContext, IdentityContext>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IIdentityService, IdentityService>();

        var identityOptionsSection = configuration.GetSection("IdentityServer");
        var identityOptions = new IdentityOptions();
        identityOptionsSection.Bind(identityOptions);

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password = identityOptions.Password;
                options.User = identityOptions.User;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
        services.AddFluentValidationAutoValidation();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "_myAllowSpecificOrigins",
                policy =>
                {
                    policy.WithOrigins("https://localhost:7095")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}