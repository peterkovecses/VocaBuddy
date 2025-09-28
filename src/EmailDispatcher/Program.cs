var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Services.AddServices(builder.Configuration);
var host = builder.Build();
host.Run();