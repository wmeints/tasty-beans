using Microsoft.AspNetCore.Mvc;
using TastyBeans.Registration.Api.Forms;
using TastyBeans.Registration.Application.CommandHandlers;
using TastyBeans.Registration.Domain.Registrations.Commands;

namespace TastyBeans.Registration.Api.Controllers;

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