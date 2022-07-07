using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Marten;
using Marten.Events;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;
using Xunit;

namespace TastyBeans.Catalog.Application.Tests.CommandHandlers;

public class UpdateProductCommandHandlerTests
{
    private IEventPublisher _eventPublisher;
    private UpdateProductCommandHandler _commandHandler;
    private IDocumentSession _documentSession;
    private IEventStore _eventStore;

    public UpdateProductCommandHandlerTests()
    {
        _documentSession = A.Fake<IDocumentSession>();
        _eventPublisher = A.Fake<IEventPublisher>();
        _eventStore = A.Fake<IEventStore>();
        
        A.CallTo(() => _documentSession.Events).Returns(_eventStore);

        _commandHandler = new UpdateProductCommandHandler(_documentSession, _eventPublisher);
    }

    [Fact]
    public async Task CanExecuteCommands()
    {
        var product = new Product(
            Guid.NewGuid(),
            "Test coffee",
            "Test description");

        A.CallTo(() => _eventStore.AggregateStreamAsync<Product>(A<Guid>.Ignored, A<long>.Ignored,
                A<DateTimeOffset?>.Ignored, A<Product?>.Ignored, A<long>.Ignored, A<CancellationToken>.Ignored))
            .Returns(product);

        var command = new UpdateProductCommand(product.Id, "Test", "Test");

        var response = await _commandHandler.Handle(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<object>>.Ignored)).MustHaveHappened();

        response.Should().NotBeNull();
    }
}