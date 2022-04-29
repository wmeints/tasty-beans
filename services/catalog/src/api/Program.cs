using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Application.QueryHandlers;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Infrastructure.Persistence;
using RecommendCoffee.Shared.Diagnostics;
using RecommendCoffee.Shared.Infrastructure.EventBus;

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

builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "RecommendCoffee.Catalog.Api",
    "RecommendCoffee.Catalog.Application",
    "RecommendCoffee.Catalog.Domain",
    "RecommendCoffee.Catalog.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "RecommendCoffee.Catalog.Api",
    "RecommendCoffee.Catalog.Application",
    "RecommendCoffee.Catalog.Domain",
    "RecommendCoffee.Catalog.Infrastructure");

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "catalog.deadletter.v1");
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<RegisterProductCommandHandler>();
builder.Services.AddScoped<UpdateProductCommandHandler>();
builder.Services.AddScoped<TasteTestProductCommandHandler>();
builder.Services.AddScoped<DiscontinueProductCommandHandler>();
builder.Services.AddScoped<FindProductByIdQueryHandler>();
builder.Services.AddScoped<FindAllProductsQueryHandler>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapControllers();

app.Run();