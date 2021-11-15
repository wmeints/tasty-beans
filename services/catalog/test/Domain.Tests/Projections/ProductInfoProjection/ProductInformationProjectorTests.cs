using Moq;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RecommendCoffee.Catalog.Domain.Tests.Projections.ProductInfoProjection;

public class ProductInformationProjectorTests
{
    [Fact]
    public async Task ApplyDomainEvents_ProductRegistered_InsertsRecord()
    {
        var repository = new Mock<IProductInformationRepository>();
        var projector = new ProductInformationProjector(repository.Object);

        await projector.ApplyEvents(new[]
        {
            new ProductRegistered(Guid.NewGuid(), "test", "test", new[] { new ProductVariant(250, 7.5M)})
        });

        repository.Verify(x => x.Insert(It.IsAny<ProductInformation>()));
    }
}
