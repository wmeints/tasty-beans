using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using RecommendCoffee.Catalog.Api.Models.Requests;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Application.Persistence;
using RecommendCoffee.Catalog.Application.QueryHandlers;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Common;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;
using RecommendCoffee.Catalog.Infrastructure.Persistence;
using RecommendCoffee.Catalog.Application.IntegrationEvents;
using RecommendCoffee.Catalog.Infrastructure.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDatabase"));
});

builder.Services.AddScoped<IEventStore<Product, Guid>, EventStore<Product, Guid>>();
builder.Services.AddScoped<IProductInformationRepository, ProductInformationRepository>();
builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddScoped<ProductInformationProjector>();
builder.Services.AddScoped<RegisterProductCommandHandler>();
builder.Services.AddScoped<FindAllProductsQueryHandler>();
builder.Services.AddScoped<FindProductQueryHandler>();

builder.Services.AddMvc();
builder.Services.AddProblemDetails(options =>
{
    options.Map<BusinessRulesViolationException>(ex =>
    {
        var details = new ValidationProblemDetails
        {
            Type = "https://recommend.coffee/problems/business-rules-violation",
            Title = ex.Message
        };

        var violations = ex.Errors.GroupBy(
            x => x.Property,
            (key, values) => (key, values.Select(x => x.Message).ToArray())
        );

        foreach (var (propertyName, errorMessages) in violations)
        {
            details.Errors.Add(propertyName, errorMessages);
        }

        return details;
    });

    options.Map<AggregateNotFoundException>(ex => new ProblemDetails
    {
        Status = 404,
        Type = "https://httpstatuses.com/404",
        Title = ex.Message
    });
});

var app = builder.Build();

app.UseProblemDetails();

app.MapGet("/products", async (FindAllProductsQueryHandler queryHandler, int page) =>
{
    return await queryHandler.Execute(new FindAllProductsQuery(page, 20));
});

app.MapGet("/products/{id}", async (FindProductQueryHandler queryHandler, Guid id) =>
{
    var result = await queryHandler.Execute(new FindProductQuery(id));

    if (result.Product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result.Product);
}).WithName("GetProductDetails");

app.MapPost("/products", async (RegisterProductCommandHandler commandHandler, RegisterProductRequest request) =>
{
    var result = await commandHandler.Execute(request.ToCommand());
    return Results.CreatedAtRoute("GetProductDetails", new { id = result.ProductId });
});

app.MapPut("/products/{id}", async (UpdateProductDetailsCommandHandler commandHandler, Guid id, UpdateProductDetailsRequest request) =>
{
    await commandHandler.Execute(request.ToCommand(id));
    return Results.AcceptedAtRoute("GetProductDetails", new { id });
});

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.Run();
