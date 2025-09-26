var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();