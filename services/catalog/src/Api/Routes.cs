using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Catalog.Api.Models.Requests;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Application.QueryHandlers;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;

namespace RecommendCoffee.Catalog.Api;

public static class Routes
{
    public static async Task<FindAllProductsQueryResult> FindAllProducts(
        [FromServices] FindAllProductsQueryHandler queryHandler, 
        [FromQuery] int page)
    {
        return await queryHandler.Execute(new FindAllProductsQuery(page, 20));
    }

    public static async Task<IResult> FindProductById(
        [FromServices] FindProductQueryHandler queryHandler, 
        [FromRoute] Guid id)
    {
        var result = await queryHandler.Execute(new FindProductQuery(id));

        if (result.Product == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result.Product);
    }

    public static async Task<IResult> RegisterProduct(
        [FromServices] RegisterProductCommandHandler commandHandler, 
        [FromBody] RegisterProductRequest request)
    {
        var result = await commandHandler.Execute(request.ToCommand());

        return Results.CreatedAtRoute("GetProductDetails", new
        {
            id = result.ProductId
        });
    }

    public static async Task<IResult> UpdateProductDetails(
        [FromBody] UpdateProductDetailsRequest request, 
        [FromServices] UpdateProductDetailsCommandHandler commandHandler, 
        [FromRoute] Guid id)
    {
        await commandHandler.Execute(request.ToCommand(id));

        return Results.AcceptedAtRoute("GetProductDetails", new
        {
            id
        });
    }
}
