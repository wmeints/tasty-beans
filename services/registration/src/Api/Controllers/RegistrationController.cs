using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Registration.Api.Forms;
using RecommendCoffee.Registration.Application.CommandHandlers;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Registrations.Commands;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Api.Controllers;

[ApiController]
[Route("/")]
public class RegistrationController : ControllerBase
{
    private readonly StartRegistrationCommandHandler _startRegistrationCommandHandler;

    public RegistrationController(StartRegistrationCommandHandler startRegistrationCommandHandler)
    {
        _startRegistrationCommandHandler = startRegistrationCommandHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Start(StartRegistrationForm form)
    {
        var customerId = Guid.NewGuid();
        
        var command = new StartRegistrationCommand(
            customerId,
            form.CustomerDetails,
            form.Subscription,
            form.PaymentMethod);

        await _startRegistrationCommandHandler.ExecuteAsync(command);
        
        return Ok(new { CustomerId = customerId });
    }
}