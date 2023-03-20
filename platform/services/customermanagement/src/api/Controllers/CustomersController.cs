using Microsoft.AspNetCore.Mvc;
using TastyBeans.CustomerManagement.Api.Common;
using TastyBeans.CustomerManagement.Api.Forms;
using TastyBeans.CustomerManagement.Application.CommandHandlers;
using TastyBeans.CustomerManagement.Application.QueryHandlers;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Shared.Domain;

namespace TastyBeans.CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly FindAllCustomersQueryHandler _findAllCustomersQueryHandler;
        private readonly FindCustomerQueryHandler _findCustomerQueryHandler;
        private readonly RegisterCustomerCommandHandler _registerCustomerCommandHandler;

        public CustomersController(
            FindAllCustomersQueryHandler findAllCustomersQueryHandler,
            FindCustomerQueryHandler findCustomerQueryHandler,
            RegisterCustomerCommandHandler registerCustomerCommandHandler)
        {
            _findAllCustomersQueryHandler = findAllCustomersQueryHandler;
            _findCustomerQueryHandler = findCustomerQueryHandler;
            _registerCustomerCommandHandler = registerCustomerCommandHandler;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Customer>>> Index(int page = 0)
        {
            var result = await _findAllCustomersQueryHandler.ExecuteAsync(page, 20);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Details(Guid id)
        {
            var result = await _findCustomerQueryHandler.ExecuteAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterCustomerForm form)
        {
            var command = new RegisterCustomerCommand(
                form.CustomerId,
                form.FirstName,
                form.LastName,
                form.InvoiceAddress,
                form.ShippingAddress,
                form.EmailAddress,
                form.TelephoneNumber);

            var response = await _registerCustomerCommandHandler.ExecuteAsync(command);

            if (!response.IsValid)
            {
                ModelState.AddValidationErrors(response.Errors);
                return BadRequest(ModelState);
            }

            return Ok(response.Customer);
        }
    }
}
