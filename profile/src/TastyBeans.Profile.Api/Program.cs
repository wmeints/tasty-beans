using TastyBeans.Profile.Api.Application.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Host.UseJasper(options =>
{
    options
        .ListenToRabbitQueue("profile")
        .UsePersistentInbox();

    options
        .UseRabbitMq(rabbit =>
        {
            rabbit.HostName = builder.Configuration["EventBus:HostName"];
            rabbit.UserName = builder.Configuration["EventBus:UserName"];
            rabbit.Password = builder.Configuration["EventBus:Password"];
            rabbit.Port = Int32.Parse(builder.Configuration["EventBus:Port"]);
        });

    options
        .PublishMessage<CustomerRegisteredIntegrationEvent>()
        .ToRabbit("profile.customer.registered.v1", "events")
        .UsePersistentOutbox();
    
    options
        .PublishMessage<SubscriptionCancelledIntegrationEvent>()
        .ToRabbit("profile.customer.subscription-cancelled.v1", "events")
        .UsePersistentOutbox();
    
    options
        .PublishMessage<SubscriptionEndedIntegrationEvent>()
        .ToRabbit("profile.customer.subscription-ended.v1", "events")
        .UsePersistentOutbox();
    
    options
        .PublishMessage<SubscriptionStartedIntegrationEvent>()
        .ToRabbit("profile.customer.subscription-started.v1", "events")
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

        options.Projections.Add<CancelledSubscriptionProjection>();
        options.Projections.Add<CustomerInfoProjection>();
        options.Projections.Add<SubscriptionHistoryProjection>();
    })
    .AddAsyncDaemon(DaemonMode.HotCold)
    .IntegrateWithJasper()
    .ApplyAllDatabaseChangesOnStartup();

var app = builder.Build();

app.MapControllers();

app.Run();