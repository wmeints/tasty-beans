using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

namespace RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;

public class ProductInformationProjector : EventProjector
{
    private readonly IProductInformationRepository _productInformationRepository;

    public ProductInformationProjector(IProductInformationRepository productInformationRepository)
    {
        _productInformationRepository = productInformationRepository;
    }

    protected override async Task ApplyEvent(Event @event)
    {
        switch (@event)
        {
            case ProductRegistered evt:
                await OnProductRegistered(evt);
                break;
        }
    }

    private async Task OnProductRegistered(ProductRegistered evt)
    {
        var variants = evt.Variants
            .Select(x => new ProductVariantInformation(x.Weight, x.UnitPrice))
            .ToList();

        await _productInformationRepository.Insert(new ProductInformation(
            evt.ProductId,
            evt.Name,
            evt.Description,
            variants
        ));
    }
}
