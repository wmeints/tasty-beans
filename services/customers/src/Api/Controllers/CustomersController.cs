using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.CustomerManagement.Api.Common;
using RecommendCoffee.CustomerManagement.Api.Forms;
using RecommendCoffee.CustomerManagement.Application.CommandHandlers;
using RecommendCoffee.CustomerManagement.Application.QueryHandlers;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.CustomerManagement.Domain.Common;

namespace RecommendCoffee.CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
