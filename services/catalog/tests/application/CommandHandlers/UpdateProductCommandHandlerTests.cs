using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;
using Xunit;

namespace TastyBeans.Catalog.Application.Tests.CommandHandlers;

public class UpdateProductCommandHandlerTests
{
    private IProductRepository _productRepository;
    private IEventPublisher _eventPublisher;

    private UpdateProductCommandHandler _commandHandler;
        
    
    public UpdateProductCommandHandlerTests()
    {
        _productRepository = A.Fake<IProductRepository>();
        _eventPublisher = A.Fake<IEventPublisher>();
        _commandHandler = new UpdateProductCommandHandler(_productRepository, _eventPublisher);
    }

    [Fact]
    public async Task CanExecuteCommands()
    {
        var product = new Product(
            Guid.NewGuid(),
            "Test coffee",
            "Test description",
            new ProductVariant[]
            {
                new ProductVariant(250, 5.95m)
            });

        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns(product);

        var command = new UpdateProductCommand(product.Id, "Test", "Test", new[]
        {
            new ProductVariant(500, 12.45m)
        });

        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _productRepository.UpdateAsync(A<Product>.Ignored)).MustHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();

        response.Should().NotBeNull();
    }

    [Fact]
    public async Task DoesntPublishEventsIfResponseIsInvalid()
    {
        var product = new Product(
            Guid.NewGuid(),
            "Test coffee",
            "Test description",
            new ProductVariant[]
            {
                new ProductVariant(250, 5.95m)
            });

        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns(product);

        var command = new UpdateProductCommand(product.Id, "Test", "Test", new List<ProductVariant>());

        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _productRepository.UpdateAsync(A<Product>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustNotHaveHappened();

        response.Should().NotBeNull();
    }

    [Fact]
    public async Task ThrowsExceptionWhenEntityIsNotFound()
    {
        var product = new Product(
            Guid.NewGuid(),
            "Test coffee",
            "Test description",
            new ProductVariant[]
            {
                new ProductVariant(250, 5.95m)
            });

        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns((Product?)null);

        var command = new UpdateProductCommand(product.Id, "Test", "Test", new List<ProductVariant>());

        await Assert.ThrowsAsync<AggregateNotFoundException>(async () =>
        {
            var response = await _commandHandler.ExecuteAsync(command);
        });

    }
}
