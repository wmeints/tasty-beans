using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Ratings.Application.CommandHandlers;
using TastyBeans.Ratings.Application.EventHandlers;
using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate;
using TastyBeans.Ratings.Infrastructure.Persistence;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;

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
    "TastyBeans.Ratings.Api",
    "TastyBeans.Ratings.Application",
    "TastyBeans.Ratings.Domain",
    "TastyBeans.Ratings.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Ratings.Api",
    "TastyBeans.Ratings.Application",
    "TastyBeans.Ratings.Domain",
    "TastyBeans.Ratings.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "ratings.deadletter.v1");
builder.Services.AddScoped<ProductRegisteredEventHandler>();
builder.Services.AddScoped<ProductUpdatedEventHandler>();
builder.Services.AddScoped<ProductDiscontinuedEventHandler>();
builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<RegisterRatingCommandHandler>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

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
