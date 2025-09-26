var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();