using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Shared.Diagnostics;
using RecommendCoffee.Shared.Infrastructure.EventBus;
using RecommendCoffee.Subscriptions.Api;
using RecommendCoffee.Subscriptions.Api.Services;
using RecommendCoffee.Subscriptions.Application.CommandHandlers;
using RecommendCoffee.Subscriptions.Application.EventHandlers;
using RecommendCoffee.Subscriptions.Application.QueryHandlers;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Services.Recommendations;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping;
using RecommendCoffee.Subscriptions.Infrastructure.Agents.Recommendations;
using RecommendCoffee.Subscriptions.Infrastructure.Agents.Shipping;
using RecommendCoffee.Subscriptions.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase"), 
        opts => opts.EnableRetryOnFailure());
});

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

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"))
    .AddDbContextCheck<ApplicationDbContext>();


var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "RecommendCoffee.Subscriptions.Api",
    "RecommendCoffee.Subscriptions.Application",
    "RecommendCoffee.Subscriptions.Domain",
    "RecommendCoffee.Subscriptions.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "RecommendCoffee.Subscriptions.Api",
    "RecommendCoffee.Subscriptions.Application",
    "RecommendCoffee.Subscriptions.Domain",
    "RecommendCoffee.Subscriptions.Infrastructure");

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "subscriptions.deadletter.v1");
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IShipping, ShippingAgent>();
builder.Services.AddScoped<IRecommendations, RecommendationsAgent>();
builder.Services.AddScoped<StartSubscriptionCommandHandler>();
builder.Services.AddScoped<CancelSubscriptionCommandHandler>();
builder.Services.AddScoped<ChangeShippingFrequencyCommandHandler>();
builder.Services.AddScoped<FindSubscriptionQueryHandler>();
builder.Services.AddScoped<MonthHasPassedEventHandler>();

// Use a background task queue to process requests asynchronously from the HTTP interface.
// We need this since month-has-passed might kick off a long-running process ;-)
builder.Services.AddSingleton<BackgroundTaskQueue>();
builder.Services.AddHostedService<BackgroundTaskService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();    
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions 
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
