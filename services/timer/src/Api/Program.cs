using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RecommendCoffee.Timer.Api;
using RecommendCoffee.Timer.Application.Common;
using RecommendCoffee.Timer.Application.Services;
using RecommendCoffee.Timer.Infrastructure.EventBus;


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

builder.Services.AddHealthChecks();

builder.AddTelemetry("Timer",
    "RecommendCoffee.Timer.Api",
    "RecommendCoffee.Timer.Application",
    "RecommendCoffee.Timer.Domain",
    "RecommendCoffee.Timer.Infrastructure");

builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();

// This timer can be sped up to simulate months faster.
// We use CRON expressions here as they feel familiar :D
builder.Services.AddHostedService<MonthHasPassedTimer>(serviceProvider => new MonthHasPassedTimer(
    builder.Configuration["Timers:MonthHasPassed"],
    serviceProvider.GetRequiredService<IEventPublisher>())
);

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions 
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();