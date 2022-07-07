using System.Text.Json;
using System.Text.Json.Serialization;
using Baseline;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Application.Projections;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;
using TastyBeans.Catalog.Infrastructure.Persistence;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("EventStore"));

    options.Projections.Add<ProductInformationProjection>();
    
    options.Events.AddEventType(typeof(ProductRegisteredEvent));
    options.Events.AddEventType(typeof(ProductUpdatedEvent));
    options.Events.AddEventType(typeof(ProductTasteTestedEvent));
    options.Events.AddEventType(typeof(ProductDiscontinuedEvent));
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
        daprClientBuilder.UseJsonSerializationOptions(serializerOptions);
    });

builder.Services.AddHeaderPropagation();
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

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
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddMediatR(typeof(RegisterProductCommand).Assembly);

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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