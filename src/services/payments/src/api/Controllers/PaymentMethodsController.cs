using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;
using TastyBeans.Payments.Api.Forms;
using TastyBeans.Payments.Application.CommandHandlers;
using TastyBeans.Shared.Api;

namespace TastyBeans.Payments.Api.Controllers;

[ApiController]
[Route("/paymentmethods")]
public class PaymentMethodsController : ControllerBase
{
    private readonly ILogger<PaymentMethodsController> _logger;
    private readonly RegisterPaymentMethodCommandHandler _registerPaymentMethodCommandHandler;

    public PaymentMethodsController(RegisterPaymentMethodCommandHandler registerPaymentMethodCommandHandler, ILogger<PaymentMethodsController> logger)
    {
        _registerPaymentMethodCommandHandler = registerPaymentMethodCommandHandler;
        _logger = logger;
    }

    public async Task<IActionResult> RegisterPaymentMethod(RegisterPaymentMethodForm form)
    {
        var command = new RegisterPaymentMethodCommand(
            Guid.NewGuid(),
            form.CardType,
            form.CardNumber,
            form.ExpirationDate,
            form.SecurityCode,
            form.CardHolderName,
            form.CustomerId
        );
        
        var response = await _registerPaymentMethodCommandHandler.ExecuteAsync(command);

        ModelState.AddValidationErrors(response.Errors);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Accepted();
    }
}