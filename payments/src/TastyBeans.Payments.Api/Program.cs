using TastyBeans.Payments.Api.Application.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Host.UseJasper(options =>
{
    options
        .UseRabbitMq(rabbit =>
        {
            rabbit.HostName = builder.Configuration["EventBus:HostName"];
            rabbit.UserName = builder.Configuration["EventBus:UserName"];
            rabbit.Password = builder.Configuration["EventBus:Password"];
            rabbit.Port = Int32.Parse(builder.Configuration["EventBus:Port"]);
        });

    options
        .PublishMessage<PaymentMethodRegisteredIntegrationEvent>()
        .ToRabbit("payments.payment-method.registered.v1", "events")
        .UsePersistentOutbox();
    
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

var app = builder.Build();

app.MapControllers();

app.Run();