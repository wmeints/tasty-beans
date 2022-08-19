using Microsoft.AspNetCore.Mvc;
using TastyBeans.Payments.Api.Application.Commands;
using TastyBeans.Payments.Api.Forms;

namespace TastyBeans.Payments.Api.Controllers;

[ApiController]
[Route("/payment-methods")]
public class PaymentMethodsController: ControllerBase
{
    private readonly ICommandBus _commandBus;

    public PaymentMethodsController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPaymentMethod(RegisterPaymentMethodForm form)
    {
        var paymentMethodId = Guid.NewGuid();
        
        await _commandBus.InvokeAsync(new RegisterPaymentMethodCommand(
            paymentMethodId, 
            form.CustomerId,
            form.CardHolderName,
            form.CardNumber,
            form.ExpirationDate,
            form.SecurityCode,
            form.CardType));

        return Accepted();
    }
}