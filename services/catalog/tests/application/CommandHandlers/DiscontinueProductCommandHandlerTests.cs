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

public class DiscontinueProductCommandHandlerTests
{
    private IEventPublisher _eventPublisher;
    private DiscontinueProductCommandHandler _commandHandler;
    private IDocumentSession _documentSession;
    private IEventStore _eventStore;

    public DiscontinueProductCommandHandlerTests()
    {
        _documentSession = A.Fake<IDocumentSession>();
        _eventPublisher = A.Fake<IEventPublisher>();
        _eventStore = A.Fake<IEventStore>();
        
        A.CallTo(() => _documentSession.Events).Returns(_eventStore);

        _commandHandler = new DiscontinueProductCommandHandler(_documentSession, _eventPublisher);
    }

    [Fact]
    public async Task CanHandleCommands()
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test");

        var command = new DiscontinueProductCommand(product.Id);
        var response = await _commandHandler.Handle(command);

        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<object>>.Ignored)).MustHaveHappened();
    }
}