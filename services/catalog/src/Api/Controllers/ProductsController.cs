using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Catalog.Api.Common;
using RecommendCoffee.Catalog.Api.Forms;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Application.QueryHandlers;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : ControllerBase
{
    private readonly RegisterProductCommandHandler _registerProductCommandHandler;
    private readonly UpdateProductCommandHandler _updateProductCommandHandler;
    private readonly DiscontinueProductCommandHandler _discontinueProductCommandHandler;
    private readonly FindProductByIdQueryHandler _findProductByIdQueryHandler;
    private readonly FindAllProductsQueryHandler _findAllProductsQueryHandler;

    public ProductsController(RegisterProductCommandHandler registerProductCommandHandler,
        UpdateProductCommandHandler updateProductCommandHandler,
        DiscontinueProductCommandHandler discontinueProductCommandHandler,
        FindProductByIdQueryHandler findProductByIdQueryHandler,
        FindAllProductsQueryHandler findAllProductsQueryHandler)
    {
        _registerProductCommandHandler = registerProductCommandHandler;
        _updateProductCommandHandler = updateProductCommandHandler;
        _discontinueProductCommandHandler = discontinueProductCommandHandler;
        _findProductByIdQueryHandler = findProductByIdQueryHandler;
        _findAllProductsQueryHandler = findAllProductsQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Product>>> Index(int page = 0)
    {
        var result = await _findAllProductsQueryHandler.ExecuteAsync(page, 20);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Details(Guid id)
    {
        var result = await _findProductByIdQueryHandler.ExecuteAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(CreateProductForm form)
    {
        var command = new RegisterProductCommand(form.Name, form.Description, form.Variants);
        var response = await _registerProductCommandHandler.ExecuteAsync(command);

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
        try
        {
            var command = new UpdateProductCommand(id, form.Name, form.Description, form.Variants);
            var response = await _updateProductCommandHandler.ExecuteAsync(command);

            ModelState.AddValidationErrors(response.Errors);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Accepted();
        }
        catch (AggregateNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var command = new DiscontinueProductCommand(id);
            var response = await _discontinueProductCommandHandler.ExecuteAsync(command);

            ModelState.AddValidationErrors(response.Errors);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
        catch (AggregateNotFoundException)
        {
            return NotFound();
        }
    }
}