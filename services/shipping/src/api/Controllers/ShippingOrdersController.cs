using Microsoft.AspNetCore.Mvc;
using TastyBeans.Shared.Api;
using TastyBeans.Shipping.Api.Forms;
using TastyBeans.Shipping.Application.CommandHandlers;
using TastyBeans.Shipping.Application.QueryHandlers;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace TastyBeans.Shipping.Api.Controllers;

[ApiController]
[Route("/shippingorders")]
public class ShippingOrdersController : ControllerBase
{
    private readonly CreateShippingOrderCommandHandler _createShippingOrderCommandHandler;
    private readonly FindShippingOrderQueryHandler _findShippingOrderQueryHandler;

    public ShippingOrdersController(
        CreateShippingOrderCommandHandler createShippingOrderCommandHandler,
        FindShippingOrderQueryHandler findShippingOrderQueryHandler)
    {
        _createShippingOrderCommandHandler = createShippingOrderCommandHandler;
        _findShippingOrderQueryHandler = findShippingOrderQueryHandler;
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

    [HttpGet("{shippingOrderId}")]
    public async Task<IActionResult> Get(Guid shippingOrderId)
    {
        var shippingOrder = _findShippingOrderQueryHandler.ExecuteAsync(shippingOrderId);

        if (shippingOrder == null)
        {
            return NotFound();
        }

        return Ok(shippingOrder);
    }
}