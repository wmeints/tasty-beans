using Microsoft.AspNetCore.Mvc;
using TastyBeans.Shared.Api;
using TastyBeans.Shared.Domain;
using TastyBeans.Subscriptions.Api.Forms;
using TastyBeans.Subscriptions.Application.CommandHandlers;
using TastyBeans.Subscriptions.Application.QueryHandlers;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace TastyBeans.Subscriptions.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly StartSubscriptionCommandHandler _startSubscriptionCommandHandler;
    private readonly CancelSubscriptionCommandHandler _cancelSubscriptionCommandHandler;
    private readonly ChangeShippingFrequencyCommandHandler _changeShippingFrequencyCommandHandler;
    private readonly FindSubscriptionQueryHandler _findSubscriptionQueryHandler;

    public SubscriptionsController(StartSubscriptionCommandHandler startSubscriptionCommandHandler,
        CancelSubscriptionCommandHandler cancelSubscriptionCommandHandler,
        ChangeShippingFrequencyCommandHandler changeShippingFrequencyCommandHandler,
        FindSubscriptionQueryHandler findSubscriptionQueryHandler)
    {
        _startSubscriptionCommandHandler = startSubscriptionCommandHandler;
        _cancelSubscriptionCommandHandler = cancelSubscriptionCommandHandler;
        _changeShippingFrequencyCommandHandler = changeShippingFrequencyCommandHandler;
        _findSubscriptionQueryHandler = findSubscriptionQueryHandler;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<Subscription>> Details(Guid customerId)
    {
        var subscription = await _findSubscriptionQueryHandler.ExecuteAsync(customerId);

        if (subscription == null)
        {
            return NotFound();
        }

        return Ok(subscription);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSubscriptionForm form)
    {
        var command = new StartSubscriptionCommand(form.CustomerId, form.Kind, form.Frequency);
        var response = await _startSubscriptionCommandHandler.ExecuteAsync(command);

        if (!response.IsValid)
        {
            ModelState.AddValidationErrors(response.Errors);
            return BadRequest(ModelState);
        }

        return Ok(response.Subscription);
    }

    [HttpPut("{customerId}")]
    public async Task<IActionResult> Update(Guid customerId, UpdateSubscriptionForm form)
    {
        try
        {
            var command = new ChangeShippingFrequencyCommand(customerId, form.Frequency);
            var response =
                await _changeShippingFrequencyCommandHandler.ExecuteAsync(command);

            if (!response.IsValid)
            {
                ModelState.AddValidationErrors(response.Errors);
                return BadRequest(ModelState);
            }

            return Accepted();
        }
        catch (AggregateNotFoundException)
        {
            return NotFound();
        }
    }
}