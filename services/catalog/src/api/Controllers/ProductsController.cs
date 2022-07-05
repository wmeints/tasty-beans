using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TastyBeans.Catalog.Api.Common;
using TastyBeans.Catalog.Api.Forms;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Application.Queries;
using TastyBeans.Catalog.Application.QueryHandlers;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Product>>> Index(int page = 0)
    {
        var result = await _mediator.Send(new FindAllProducts(page, 20));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Details(Guid id)
    {
        var result = await _mediator.Send(new FindProductById(id));

        if (result.Product == null)
        {
            return NotFound();
        }

        return Ok(result.Product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(CreateProductForm form)
    {
        var response = await _mediator.Send(new RegisterProductCommand(form.Name, form.Description));

        ModelState.AddValidationErrors(response.Errors);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(response.Product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(Guid id, UpdateProductForm form)
    {
        var command = new UpdateProductCommand(id, form.Name, form.Description);
        var response = await _mediator.Send(command);

        ModelState.AddValidationErrors(response.Errors);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Accepted();
    }

    [HttpPost("{id}/taste")]
    public async Task<IActionResult> TasteTest(Guid id, TasteTestForm form)
    {
        var response = await _mediator.Send(new TasteTestProductCommand(
            id, form.RoastLevel, form.Taste, form.FlavorNotes));

        if (!response.ProductExists)
        {
            return NotFound();
        }

        ModelState.AddValidationErrors(response.Errors);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return AcceptedAtAction("Details", new {id = id});
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DiscontinueProductCommand(id);
        var response = await _mediator.Send(command);

        if (!response.ProductExists)
        {
            return NotFound();
        }

        ModelState.AddValidationErrors(response.Errors);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
}