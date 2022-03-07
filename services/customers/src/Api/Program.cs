using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.CustomerManagement.Application.CommandHandlers;
using RecommendCoffee.CustomerManagement.Application.Common;
using RecommendCoffee.CustomerManagement.Application.QueryHandlers;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.CustomerManagement.Infrastructure.EventBus;
using RecommendCoffee.CustomerManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddDaprClient();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddDapr();

builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<RegisterCustomerCommandHandler>();
builder.Services.AddScoped<FindCustomerQueryHandler>();
builder.Services.AddScoped<FindAllCustomersQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseCloudEvents();

app.MapSubscribeHandler();
app.MapControllers();

app.Run();