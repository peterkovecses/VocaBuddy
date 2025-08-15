using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Respawn;
using Testcontainers.MsSql;
using VocaBuddy.Infrastructure.Persistence;

namespace Api.IntegrationTests;

public class VocaBuddyApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Super_secret_password_123@")
        .WithName($"vocabuddy-db-{Guid.NewGuid()}")
        .WithPortBinding(1433, true)
        .Build();
    
    private Respawner _respawner = default!;
    
    public HttpClient HttpClient { get; private set; } = default!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHostedService));
            
            var descriptorType =
                typeof(DbContextOptions<VocaBuddyContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<VocaBuddyContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
            
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VocaBuddyContext>();
            dbContext.Database.Migrate();
            
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var key = Encoding.UTF8.GetBytes(JwtConstants.SecurityKey);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        });
    }
    
    public async Task ResetDatabaseAsync() => 
        await _respawner.ResetAsync(_dbContainer.GetConnectionString());

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        // if you're using a database besides SQL Server, pass an open DbConnection
        _respawner = await Respawner.CreateAsync(
            _dbContainer.GetConnectionString());
        HttpClient = CreateClient();
    }

    public new async Task DisposeAsync() => 
        await _dbContainer.DisposeAsync();
}