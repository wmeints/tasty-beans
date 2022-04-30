﻿using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Shipping.Api.Forms;
using RecommendCoffee.Shipping.Application.CommandHandlers;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace RecommendCoffee.Shipping.Api.Controllers;

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