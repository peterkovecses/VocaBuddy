var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.ConfigureServices(builder.Configuration);

await builder.Build().RunAsync();
