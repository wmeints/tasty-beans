using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Catalog.Api.Models.Requests;

public record RegisterProductRequest(string Name, string Description, IEnumerable<ProductInformationDto> Variants)
{
    public RegisterProductCommand ToCommand()
    {
        return new RegisterProductCommand(
            Name, 
            Description, 
            Variants.Select(x => new ProductVariant(x.Weight, x.UnitPrice)).ToList()
        );
    }
}
