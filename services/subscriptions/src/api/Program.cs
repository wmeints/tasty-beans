using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;
using TastyBeans.Subscriptions.Api.Services;
using TastyBeans.Subscriptions.Application.CommandHandlers;
using TastyBeans.Subscriptions.Application.EventHandlers;
using TastyBeans.Subscriptions.Application.QueryHandlers;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using TastyBeans.Subscriptions.Domain.Services.Recommendations;
using TastyBeans.Subscriptions.Domain.Services.Shipping;
using TastyBeans.Subscriptions.Infrastructure.Agents.Recommendations;
using TastyBeans.Subscriptions.Infrastructure.Agents.Shipping;
using TastyBeans.Subscriptions.Infrastructure.Persistence;

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

builder.Services.AddHeaderPropagation();
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.Subscriptions.Api",
    "TastyBeans.Subscriptions.Application",
    "TastyBeans.Subscriptions.Domain",
    "TastyBeans.Subscriptions.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Subscriptions.Api",
    "TastyBeans.Subscriptions.Application",
    "TastyBeans.Subscriptions.Domain",
    "TastyBeans.Subscriptions.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

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
