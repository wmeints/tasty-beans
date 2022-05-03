using System.Text.Json;
using System.Text.Json.Serialization;
using Akka.Actor;
using Akka.Configuration;
using Akka.Logger.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;
using TastyBeans.Simulation.Application.EventHandlers;
using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;
using TastyBeans.Simulation.Application.Services.Registration;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Simulation.Infrastructure.Agents;

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

builder.Services.AddHeaderPropagation();
builder.Services.AddHealthChecks();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.Simulation.Api",
    "TastyBeans.Simulation.Application",
    "TastyBeans.Simulation.Domain",
    "TastyBeans.Simulation.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Simulation.Api",
    "TastyBeans.Simulation.Application",
    "TastyBeans.Simulation.Domain",
    "TastyBeans.Simulation.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "simulation.deadletter.v1");
builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<DeliveryAttemptFailedEventHandler>();
builder.Services.AddScoped<DeliveryDelayedEventHandler>();
builder.Services.AddScoped<DriverOutForDeliveryEventHandler>();
builder.Services.AddScoped<ShipmentDeliveredEventHandler>();
builder.Services.AddScoped<ShipmentLostEventHandler>();
builder.Services.AddScoped<ShipmentReturnedEventHandler>();
builder.Services.AddScoped<ShipmentSentEventHandler>();
builder.Services.AddScoped<ShipmentSortedEventHandler>();
builder.Services.AddScoped<ShippingOrderCreatedEventHandler>();

builder.Services.AddSingleton(sp =>
{
    var loggingFactory = sp.GetRequiredService<ILoggerFactory>();
    LoggingLogger.LoggerFactory = loggingFactory;
    
    var actorSystem = ActorSystem.Create("transport-company", ConfigurationFactory.ParseString(
        "akka { loglevel=INFO,  loggers=[\"Akka.Logger.Extensions.Logging.LoggingLogger, Akka.Logger.Extensions.Logging\"]}"));

    return actorSystem;
});
builder.Services.AddSingleton<ISimulation, SimulationAdapter>();

builder.Services.AddHttpClient<IRegistration, RegistrationServiceAgent>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceLocations:Registration"]);
});

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

app.UseHeaderPropagation();
app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapControllers();

app.Run();
