using FluentAssertions;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;
using RecommendCoffee.Catalog.Domain.Common;
using System;
using System.Linq;
using Xunit;

namespace RecommendCoffee.Catalog.Domain.Tests.Aggregates.ProductAggregate;

public class ProductTests
{
    [Fact]
    public void LoadProduct_ValidEvents_ReturnsInstance()
    {
        var aggregateId = Guid.NewGuid();
        var domainEvent = new ProductRegistered(
            aggregateId,
            "test",
            "test",
            new[] { new ProductVariant(500, 7.71m) }
        );

        var product = Product.Load(aggregateId, new[] { domainEvent });

        product.Should().NotBeNull();
        product.Name.Should().Be("test");
        product.Description.Should().Be("test");
        product.Variants.Should().BeEquivalentTo(new[] { new ProductVariant(500, 7.71m) });
    }

    [Fact]
    public void RegisterProduct_ValidInput_ReturnsProduct()
    {
        var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(250, 7.50M)
            }));

        product.EventsToStore.Should().HaveCount(1);

        var productRegisteredEvent = product.EventsToStore.First().Should().BeOfType<ProductRegistered>();

        productRegisteredEvent.Which.ProductId.Should().Be(product.Id);
        productRegisteredEvent.Which.Name.Should().Be(product.Name);
        productRegisteredEvent.Which.Description.Should().Be(product.Description);
        productRegisteredEvent.Which.Variants.Should().HaveCount(1);
        productRegisteredEvent.Which.Variants.First().Should().Be(product.Variants.First());
    }

    [Fact]
    public void RegisterProduct_ValidInput_PublishesIntegrationEvent()
    {
        var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(250, 7.50M)
            }));

        product.EventsToPublish.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(250)]
    [InlineData(500)]
    [InlineData(1000)]
    public void RegisterProduct_ValidWeight_RegistersProduct(int weight)
    {
        var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(weight, 7.50M)
            }));

        var productRegisteredEvent = product.EventsToPublish.First().Should().BeOfType<ProductRegistered>();

        productRegisteredEvent.Subject.Variants.First().Weight.Should().Be(weight);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(249)]
    [InlineData(750)]
    [InlineData(1001)]
    public void RegisterProduct_InvalidWeight_RaisesException(int weight)
    {
        Assert.Throws<BusinessRulesViolationException>(() =>
        {
            var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(weight, 7.50M)
            }));
        });
    }

    [Fact]
    public void UpdateProductDetails_ValidInput_UpdatesState()
    {
        var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(250, 7.50M)
            }));

        product.Reset();

        product.UpdateDetails(new UpdateProductDetailsCommand(
            product.Id,
            "Bitter sweet",
            "A blend to dream of.",
            new[]
            {
                new ProductVariant(250, 5.25m)
            }));

        product.Name.Should().Be("Bitter sweet");
        product.Description.Should().Be("A blend to dream of.");
        product.Variants.Should().BeEquivalentTo(new[] { new ProductVariant(250, 5.25m) });
    }

    [Fact]
    public void UpdateProductDetails_ValidInput_GeneratesIntegrationEvent()
    {
        var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(250, 7.50M)
            }));

        product.Reset();

        product.UpdateDetails(new UpdateProductDetailsCommand(
            product.Id,
            "Bitter sweet",
            "A blend to dream of.",
            new[]
            {
                new ProductVariant(250, 5.25m)
            }));

        product.EventsToPublish.Should().HaveCount(1);
    }

    [Fact]
    public void UpdateProductDetails_ValidInput_GeneratesEventToStore()
    {
        var product = Product.Register(new RegisterProductCommand(
            "Power blend",
            "A dark roast to get you started.",
            new[]
            {
                new ProductVariant(250, 7.50M)
            }));

        product.Reset();

        product.UpdateDetails(new UpdateProductDetailsCommand(
            product.Id,
            "Bitter sweet",
            "A blend to dream of.",
            new[]
            {
                new ProductVariant(250, 5.25m)
            }));

        product.EventsToStore.Should().HaveCount(1);
    }
}
