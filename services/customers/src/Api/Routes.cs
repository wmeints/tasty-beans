using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Customers.Api.Models.Requests;
using RecommendCoffee.Customers.Application.CommandHandlers;
using RecommendCoffee.Customers.Application.QueryHandlers;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

namespace RecommendCoffee.Customers.Api;

public static class Routes
{
    public static async Task<IResult> RegisterCustomer(
        [FromServices] RegisterCustomerCommandHandler commandHandler,
        [FromBody] RegisterCustomerRequest request)
    {
        var response = await commandHandler.Execute(request.ToCommand());
        return Results.CreatedAtRoute("GetCustomerDetails", new {id = response.CustomerId});
    }

    public static async Task<FindAllCustomersQueryResult> FindAllCustomers(
        [FromServices] FindAllCustomersQueryHandler queryHandler,
        [FromQuery] int page)
    {
        return await queryHandler.Execute(new FindAllCustomersQuery(page, 20));
    }
    
    public static async Task<FindCustomerQueryResult> FindCustomerById(
        [FromServices]FindCustomerQueryHandler queryHandler,
        [FromRoute]Guid id)
    {
        return await queryHandler.Execute(new FindCustomerQuery(id));
    }
}