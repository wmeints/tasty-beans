using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Catalog.Api.Models.Requests
{
    public record UpdateProductDetailsRequest(string Name, string Description, IEnumerable<ProductVariant> Variants)
    {
        public UpdateProductDetailsCommand ToCommand(Guid id)
        {
            return new UpdateProductDetailsCommand(id, Name, Description, Variants);
        }
    }
}
