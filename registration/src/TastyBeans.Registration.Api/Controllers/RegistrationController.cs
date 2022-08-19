using Microsoft.AspNetCore.Mvc;
using TastyBeans.Registration.Api.Application.Commands;
using TastyBeans.Registration.Api.Forms;

namespace TastyBeans.Registration.Api.Controllers;

[ApiController]
[Route("registrations")]
public class RegistrationController: ControllerBase
{
    private readonly ICommandBus _commandBus;

    public RegistrationController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpPost("")]
    public async Task<IActionResult> StartRegistration(StartRegistrationForm form)
    {
        var customerId = Guid.NewGuid();
        
        await _commandBus.InvokeAsync(new StartRegistrationCommand(
            customerId,
            form.CustomerDetails,
            form.PaymentMethod));

        return Ok(new { customerId });
    }
}