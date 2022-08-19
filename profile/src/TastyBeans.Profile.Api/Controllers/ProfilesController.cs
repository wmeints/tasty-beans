using Jasper;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Profile.Api.Application.Commands;
using TastyBeans.Profile.Api.Application.Queries;
using TastyBeans.Profile.Api.Application.ReadModels;
using TastyBeans.Profile.Api.Forms;
using TastyBeans.Profile.Api.Shared;

namespace TastyBeans.Profile.Api.Controllers;

[ApiController]
[Route("/profiles")]
public class ProfilesController : ControllerBase
{
    private readonly ICommandBus _commandBus;

    public ProfilesController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpGet]
    public async Task<PagedResult<CustomerInfo>> GetCustomers(int page = 0)
    {
        var result = await _commandBus.InvokeAsync<PagedResult<CustomerInfo>>(new GetCustomersQuery(page, 20));
        return result ?? new PagedResult<CustomerInfo>(0, 20, 0, new List<CustomerInfo>());
    }

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CustomerInfo>> GetCustomerDetails(Guid customerId)
    {
        var result = await _commandBus.InvokeAsync<CustomerInfo?>(new GetCustomerDetailsQuery(customerId));

        return result switch
        {
            { } customerInfo => Ok(customerInfo),
            null => NotFound()
        };
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetSubscriptionHistory(Guid customerId)
    {
        var result = await _commandBus.InvokeAsync<IEnumerable<SubscriptionHistoryItem>?>(
            new GetSubscriptionHistoryQuery(customerId));

        return result switch
        {
            { } historyItems => Ok(historyItems),
            null => NotFound()
        };
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(RegisterCustomerForm form)
    {
        var command = new RegisterCustomerCommand(
            form.CustomerId,
            form.FirstName,
            form.LastName,
            form.ShippingAddress,
            form.InvoiceAddress,
            form.EmailAddress,
            DateTime.UtcNow);

        await _commandBus.InvokeAsync(command);

        return Accepted();
    }

    [HttpPost("{customerId:guid}/unsubscribe")]
    public async Task<IActionResult> UnsubscribeCustomer(Guid customerId)
    {
        var command = new UnsubscribeCustomerCommand(customerId, DateTime.UtcNow);
        await _commandBus.InvokeAsync(command);

        return Accepted();
    }

    [HttpPost("{customerId:guid}/subscribe")]
    public async Task<IActionResult> SubscribeCustomer(Guid customerId)
    {
        var command = new SubscribeCustomerCommand(customerId, DateTime.UtcNow);
        await _commandBus.InvokeAsync(command);

        return Accepted();
    }
}