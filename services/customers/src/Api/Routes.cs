using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Customers.Api.Models.Requests;
using RecommendCoffee.Customers.Application.CommandHandlers;

namespace RecommendCoffee.Customers.Api;

public static class Routes
{
    public static async Task<IResult> RegisterCustomer([FromServices] RegisterCustomerCommandHandler commandHandler, [FromBody] RegisterCustomerRequest request)
    {
        var response = await commandHandler.Execute(request.ToCommand());
        return Results.CreatedAtRoute("GetCustomerDetails", new { id = response.CustomerId });
    }
}
