using Microsoft.AspNetCore.Mvc;
using TastyBeans.Shared.Api;
using TastyBeans.Shipping.Api.Forms;
using TastyBeans.Shipping.Application.CommandHandlers;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace TastyBeans.Shipping.Api.Controllers;

[ApiController]
[Route("/shippingorders")]
public class ShippingOrdersController: ControllerBase
{
    private readonly CreateShippingOrderCommandHandler _createShippingOrderCommandHandler;

    public ShippingOrdersController(CreateShippingOrderCommandHandler createShippingOrderCommandHandler)
    {
        _createShippingOrderCommandHandler = createShippingOrderCommandHandler;
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateShippingOrderForm form)
    {
        var createShippingOrderCommand = new CreateShippingOrderCommand(form.CustomerId, form.OrderItems);
        var response = await _createShippingOrderCommandHandler.ExecuteAsync(createShippingOrderCommand);
        
        ModelState.AddValidationErrors(response.Errors);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(response.Order);
    }
}