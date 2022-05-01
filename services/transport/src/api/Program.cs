using System.Text.Json;
using System.Text.Json.Serialization;
using Akka.Actor;
using Akka.Configuration;
using Akka.Logger.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;
using TastyBeans.Transport.Application.CommandHandlers;
using TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate;
using TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Commands;
using TastyBeans.Transport.Infrastructure.EventBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddDapr(daprClientBuilder =>
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        serializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Bind the settings for the shipment actors using the configuration provided to the application.
builder.Services.Configure<TransportServiceLevelOptions>(
    builder.Configuration.GetSection("ServiceLevels"));

builder.Services.AddSingleton(sp =>
{
    var loggingFactory = sp.GetRequiredService<ILoggerFactory>();
    var eventPublisher = sp.GetRequiredService<IEventPublisher>();
    
    LoggingLogger.LoggerFactory = loggingFactory;
    
    var actorSystem = ActorSystem.Create("transport-company", ConfigurationFactory.ParseString(
        "akka { loglevel=INFO,  loggers=[\"Akka.Logger.Extensions.Logging.LoggingLogger, Akka.Logger.Extensions.Logging\"]}"));

    actorSystem.ActorOf(EventBusAdapter.Props(eventPublisher), "event-bus-adapter");
    
    return actorSystem;
});

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "transport.deadletter.v1");

builder.Services.AddHeaderPropagation();
builder.Services.AddHealthChecks();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.Catalog.Api",
    "TastyBeans.Catalog.Application",
    "TastyBeans.Catalog.Domain",
    "TastyBeans.Catalog.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Catalog.Api",
    "TastyBeans.Catalog.Application",
    "TastyBeans.Catalog.Domain",
    "TastyBeans.Catalog.Infrastructure");

builder.Services.AddLogging(telemetryOptions);


builder.Services.AddScoped<CreateShipmentCommandHandler>();

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

app.UseHeaderPropagation();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapPost("/orders", async ([FromServices] CreateShipmentCommandHandler commmandHandler, [FromBody] CreateShipmentCommand cmd) =>
{
    await commmandHandler.ExecuteAsync(cmd);
    return Results.Accepted();
});

app.MapControllers();

app.Run();
