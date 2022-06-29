using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Application.QueryHandlers;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;
using TastyBeans.Shared.Infrastructure.EventStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEventStore(options =>
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
builder.Services.AddHealthChecks().AddDbContextCheck<EventStoreDbContext>();

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

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "catalog.deadletter.v1");

builder.Services.AddScoped<RegisterProductCommandHandler>();
builder.Services.AddScoped<CompleteTasteTestCommandHandler>();
builder.Services.AddScoped<DiscontinueProductCommandHandler>();
builder.Services.AddScoped<FindProductByIdQueryHandler>();
builder.Services.AddScoped<FindAllProductsQueryHandler>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<EventStoreDbContext>();

await dbContext.Database.MigrateAsync();

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