using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using TastyBeans.CustomerManagement.Application.CommandHandlers;
using TastyBeans.CustomerManagement.Application.QueryHandlers;
using TastyBeans.CustomerManagement.Application.Services;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using TastyBeans.CustomerManagement.Infrastructure.Persistence;
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

        daprClientBuilder.UseJsonSerializationOptions(serializerOptions);
    });

builder.Services.AddHeaderPropagation();
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.CustomerManagement.Api",
    "TastyBeans.CustomerManagement.Application",
    "TastyBeans.CustomerManagement.Domain",
    "TastyBeans.CustomerManagement.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.CustomerManagement.Api",
    "TastyBeans.CustomerManagement.Application",
    "TastyBeans.CustomerManagement.Domain",
    "TastyBeans.CustomerManagement.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "customermanagement.deadletter.v1");
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<RegisterCustomerCommandHandler>();
builder.Services.AddScoped<FindCustomerQueryHandler>();
builder.Services.AddScoped<FindAllCustomersQueryHandler>();

builder.Services.AddScoped<ICustomerGenerationService, CustomerGenerationService>();
builder.Services.AddScoped<ICustomerSampleRepository, CustomerSampleRepository>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

// We use seed data to prep the environment for use with MLOps.
var customerGenerationService = scope.ServiceProvider.GetRequiredService<ICustomerGenerationService>();
await customerGenerationService.GenerateAsync("SeedData/customers.csv");

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHeaderPropagation();
app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();