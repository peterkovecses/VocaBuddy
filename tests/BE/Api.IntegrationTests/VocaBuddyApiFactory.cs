namespace Api.IntegrationTests;

public class VocaBuddyApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Super_secret_password_123@")
        .WithName($"vocabuddy-db-{Guid.NewGuid()}")
        .WithPortBinding(1433, true)
        .WithName("vocabuddy-test-db")
        .Build();
    
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    
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
                options.UseSqlServer(_dbContainer.GetConnectionString())
                       .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
            
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
        await _respawner.ResetAsync(_dbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var connectionString = _dbContainer.GetConnectionString();
        _dbConnection = new SqlConnection(connectionString);
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VocaBuddyContext>();
        await dbContext.Database.MigrateAsync();
        await InitializeRespawnerAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer
            });
    }

    public new async Task DisposeAsync() => 
        await _dbContainer.DisposeAsync();
}