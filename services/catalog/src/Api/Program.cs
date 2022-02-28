using Infrastructure.DomainEvents;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Application.QueryHandlers;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();

builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<RegisterProductCommandHandler>();
builder.Services.AddScoped<UpdateProductCommandHandler>();
builder.Services.AddScoped<DiscontinueProductCommandHandler>();
builder.Services.AddScoped<FindProductByIdQueryHandler>();
builder.Services.AddScoped<FindAllProductsQueryHandler>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.MapControllers();

app.Run();
