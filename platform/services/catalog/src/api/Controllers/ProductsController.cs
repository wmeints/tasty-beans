﻿using Microsoft.AspNetCore.Mvc;
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
    private readonly UpdateProductCommandHandler _updateProductCommandHandler;
    private readonly DiscontinueProductCommandHandler _discontinueProductCommandHandler;
    private readonly FindProductByIdQueryHandler _findProductByIdQueryHandler;
    private readonly FindAllProductsQueryHandler _findAllProductsQueryHandler;
    private readonly TasteTestProductCommandHandler _tasteTestProductCommandHandler;

    public ProductsController(RegisterProductCommandHandler registerProductCommandHandler,
        UpdateProductCommandHandler updateProductCommandHandler,
        DiscontinueProductCommandHandler discontinueProductCommandHandler,
        FindProductByIdQueryHandler findProductByIdQueryHandler,
        FindAllProductsQueryHandler findAllProductsQueryHandler, 
        TasteTestProductCommandHandler tasteTestProductCommandHandler)
    {
        _registerProductCommandHandler = registerProductCommandHandler;
        _updateProductCommandHandler = updateProductCommandHandler;
        _discontinueProductCommandHandler = discontinueProductCommandHandler;
        _findProductByIdQueryHandler = findProductByIdQueryHandler;
        _findAllProductsQueryHandler = findAllProductsQueryHandler;
        _tasteTestProductCommandHandler = tasteTestProductCommandHandler;
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

    [HttpPost("{id}/taste")]
    public async Task<IActionResult> TasteTest(Guid id, TasteTestForm form)
    {
        try
        {
            var command = new TasteTestProductCommand(id, form.RoastLevel, form.Taste, form.FlavorNotes);
            var response = await _tasteTestProductCommandHandler.ExecuteAsync(command);
            
            ModelState.AddValidationErrors(response.Errors);
            
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