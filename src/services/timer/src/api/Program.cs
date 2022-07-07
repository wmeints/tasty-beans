using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;
using TastyBeans.Timer.Application.Services;

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
    "TastyBeans.Timer.Api",
    "TastyBeans.Timer.Application",
    "TastyBeans.Timer.Domain",
    "TastyBeans.Timer.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Timer.Api",
    "TastyBeans.Timer.Application",
    "TastyBeans.Timer.Domain",
    "TastyBeans.Timer.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "timer.deadletter.v1");

// This timer can be sped up to simulate months faster.
// We use CRON expressions here as they feel familiar :D
builder.Services.AddHostedService<MonthHasPassedTimer>(serviceProvider => new MonthHasPassedTimer(
    builder.Configuration["Timers:MonthHasPassed"],
    serviceProvider.GetRequiredService<IEventPublisher>(),
    serviceProvider.GetRequiredService<ILogger<MonthHasPassedTimer>>())
);

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHeaderPropagation();
app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions 
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
