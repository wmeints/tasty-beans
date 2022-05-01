using Microsoft.AspNetCore.Mvc;
using TastyBeans.Recommendations.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Recommendations.Api.Controllers;

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