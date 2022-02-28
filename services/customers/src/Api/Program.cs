using Microsoft.EntityFrameworkCore;
using RecommendCoffee.CustomerManagement.Application.CommandHandlers;
using RecommendCoffee.CustomerManagement.Application.Common;
using RecommendCoffee.CustomerManagement.Application.QueryHandlers;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.CustomerManagement.Infrastructure.DomainEvents;
using RecommendCoffee.CustomerManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();

builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<RegisterCustomerCommandHandler>();
builder.Services.AddScoped<FindCustomerQueryHandler>();
builder.Services.AddScoped<FindAllCustomersQueryHandler>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.MapControllers();

app.Run();
