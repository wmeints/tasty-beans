using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;
using Xunit;

namespace TastyBeans.Catalog.Application.Tests.CommandHandlers;

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
        var product = new Product(Guid.NewGuid(), "Test", "Test");

        A.CallTo(() => _productRepository.FindByIdAsync(A<Guid>.Ignored)).Returns(product);

        var command = new DiscontinueProductCommand(product.Id);
        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<object>>.Ignored)).MustHaveHappened();
        A.CallTo(() => _productRepository.UpdateAsync(A<Product>.Ignored)).MustHaveHappened();
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