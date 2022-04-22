using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Recommendations.Api.Controllers;

[ApiController]
[Route("/recommendation")]
public class RecommendationController: ControllerBase
{
    private readonly IProductRepository _productRepository;

    public RecommendationController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var product = await _productRepository.GetRandomProductAsync();
        return Ok(new { ProductId = product.Id });
    }
}