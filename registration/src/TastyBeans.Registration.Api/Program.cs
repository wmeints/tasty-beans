

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Host.UseJasper(options =>
{
    options
        .ListenToRabbitQueue("registration")
        .UsePersistentInbox();

    options
        .UseRabbitMq(rabbit =>
        {
            rabbit.HostName = builder.Configuration["EventBus:HostName"];
            rabbit.UserName = builder.Configuration["EventBus:UserName"];
            rabbit.Password = builder.Configuration["EventBus:Password"];
            rabbit.Port = Int32.Parse(builder.Configuration["EventBus:Port"]);
        });
    
    options.Handlers
        .OnException<NpgsqlException>()
        .RetryWithCooldown(50.Milliseconds(), 100.Milliseconds(), 250.Milliseconds());

    options.Handlers.OnException<ConcurrencyException>().RetryTimes(3);
});

builder.Services
    .AddMarten(options =>
    {
        options.Connection(builder.Configuration["ConnectionStrings:DefaultDatabase"]);
        options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
    })
    .AddAsyncDaemon(DaemonMode.HotCold)
    .IntegrateWithJasper()
    .ApplyAllDatabaseChangesOnStartup();

builder.Services.AddHttpClient<IProfileService, ProfileService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceRegistry:Profile"]);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetExponentialBackOffPolicy());

builder.Services.AddHttpClient<IPaymentsService, PaymentsService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ServiceRegistry:Payments"]);
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(GetExponentialBackOffPolicy());

var app = builder.Build();

app.MapControllers();

app.Run();
