using Microsoft.AspNetCore.Mvc;
using TastyBeans.Catalog.Api.Common;
using TastyBeans.Catalog.Api.Forms;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Application.QueryHandlers;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : ControllerBase
{
    private readonly RegisterProductCommandHandler _registerProductCommandHandler;
    private readonly DiscontinueProductCommandHandler _discontinueProductCommandHandler;
    private readonly FindProductByIdQueryHandler _findProductByIdQueryHandler;
    private readonly FindAllProductsQueryHandler _findAllProductsQueryHandler;
    private readonly CompleteTasteTestCommandHandler _completeTasteTestCommandHandler;

    public ProductsController(RegisterProductCommandHandler registerProductCommandHandler,
        DiscontinueProductCommandHandler discontinueProductCommandHandler,
        FindProductByIdQueryHandler findProductByIdQueryHandler,
        FindAllProductsQueryHandler findAllProductsQueryHandler, 
        CompleteTasteTestCommandHandler completeTasteTestCommandHandler)
    {
        _registerProductCommandHandler = registerProductCommandHandler;
        _discontinueProductCommandHandler = discontinueProductCommandHandler;
        _findProductByIdQueryHandler = findProductByIdQueryHandler;
        _findAllProductsQueryHandler = findAllProductsQueryHandler;
        _completeTasteTestCommandHandler = completeTasteTestCommandHandler;
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
        var productId = Guid.NewGuid();
        var command = new Register(productId, form.Name, form.Description);
        await _registerProductCommandHandler.ExecuteAsync(command);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return CreatedAtAction("Details", new { id = productId });
    }

    [HttpPost("{id}/taste-test-results")]
    public async Task<IActionResult> TasteTest(Guid id, TasteTestForm form)
    {
        try
        {
            var command = new CompleteTasteTest(id, form.RoastLevel, form.Taste, form.FlavorNotes);
            await _completeTasteTestCommandHandler.ExecuteAsync(command);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return AcceptedAtAction("Details", new { id = id });
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
            var command = new Discontinue(id);
            await _discontinueProductCommandHandler.ExecuteAsync(command);

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