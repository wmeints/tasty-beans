using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Shared.Application;
using RecommendCoffee.Shared.Domain;
using Xunit;

namespace RecommendCoffee.Catalog.Application.Tests.CommandHandlers;

public class DiscontinueProductCommandHandlerTests
{
    private IProductRepository _productRepository;
    private IEventPublisher _eventPublisher;
    private DiscontinueProductCommandHandler _commandHandler;

    public DiscontinueProductCommandHandlerTests()
    {
        _productRepository = A.Fake<IProductRepository>();
        _eventPublisher = A.Fake<IEventPublisher>();
        _commandHandler = new DiscontinueProductCommandHandler(_productRepository, _eventPublisher);
    }

    [Fact]
    public async Task CanHandleCommands()
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new[]
        {
            new ProductVariant(250, 5.95m)
        });

        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns(product);

        var command = new DiscontinueProductCommand(product.Id);
        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
        A.CallTo(() => _productRepository.UpdateAsync(A<Product>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public async Task DoesntPublishEventIfCommandIsInvalid()
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new[]
        {
            new ProductVariant(250, 5.95m)
        });

        // Discontinue the product once before executing the actual command through the handler.
        product.Discontinue(new DiscontinueProductCommand(product.Id));

        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns(product);

        var command = new DiscontinueProductCommand(product.Id);
        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _productRepository.UpdateAsync(A<Product>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task ThrowsExceptionWhenEntityDoesntExist()
    {
        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns((Product?)null);

        var command = new DiscontinueProductCommand(Guid.NewGuid());

        await Assert.ThrowsAsync<AggregateNotFoundException>(async () =>
        {
            var response = await _commandHandler.ExecuteAsync(command);
        });
    }
}