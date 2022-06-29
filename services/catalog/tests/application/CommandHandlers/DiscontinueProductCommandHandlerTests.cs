using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;
using Xunit;

namespace TastyBeans.Catalog.Application.Tests.CommandHandlers;

public class DiscontinueProductCommandHandlerTests
{
    private IEventStore _eventStore;
    private IEventPublisher _eventPublisher;
    private DiscontinueProductCommandHandler _commandHandler;

    public DiscontinueProductCommandHandlerTests()
    {
        _eventStore = A.Fake<IEventStore>();
        _eventPublisher = A.Fake<IEventPublisher>();
        _commandHandler = new DiscontinueProductCommandHandler(_eventStore, _eventPublisher);
    }

    [Fact]
    public async Task CanHandleCommands()
    {
        var productId = Guid.NewGuid();
        var product = new Product(productId, 1, new IDomainEvent[]
        {
            new Registered(productId, "Test", "Test")
        });

        A.CallTo(() => _eventStore.GetAsync<Product>(A<Guid>.Ignored)).Returns(product);

        var command = new Discontinue(product.Id);
        await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
        A.CallTo(() => _eventStore.AppendAsync(A<Guid>.Ignored, A<long>.Ignored, A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public async Task DoesntPublishEventIfCommandIsInvalid()
    {
        var productId = Guid.NewGuid();
        var product = new Product(productId, 1, new IDomainEvent[]
        {
            new Registered(productId, "Test", "Test")
        });

        // Discontinue the product once before executing the actual command through the handler.
        product.Discontinue(new Discontinue(product.Id));

        A.CallTo(() => _eventStore.GetAsync<Product>(A<Guid>.Ignored)).Returns(product);

        var command = new Discontinue(product.Id);
        await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _eventStore.AppendAsync(A<Guid>.Ignored, A<long>.Ignored, A<IEnumerable<IDomainEvent>>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task ThrowsExceptionWhenEntityDoesntExist()
    {
        A.CallTo(() => _eventStore.GetAsync<Product>(A<Guid>.Ignored)).Returns((Product?)null);

        var command = new Discontinue(Guid.NewGuid());

        await Assert.ThrowsAsync<AggregateNotFoundException>(async () =>
        {
            await _commandHandler.ExecuteAsync(command);
        });
    }
}