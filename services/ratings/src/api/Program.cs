using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Ratings.Api;
using RecommendCoffee.Ratings.Application.CommandHandlers;
using RecommendCoffee.Ratings.Application.EventHandlers;
using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate;
using RecommendCoffee.Ratings.Infrastructure.Persistence;
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
    "RecommendCoffee.Ratings.Api",
    "RecommendCoffee.Ratings.Application",
    "RecommendCoffee.Ratings.Domain",
    "RecommendCoffee.Ratings.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "RecommendCoffee.Ratings.Api",
    "RecommendCoffee.Ratings.Application",
    "RecommendCoffee.Ratings.Domain",
    "RecommendCoffee.Ratings.Infrastructure");

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

if(app.Environment.IsDevelopment())
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
