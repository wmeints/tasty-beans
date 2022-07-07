using System;
using System.Collections.Generic;
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

public class RegisterProductCommandHandlerTests
{
    private IEventPublisher _eventPublisher;
    private RegisterProductCommandHandler _commandHandler;
    private IDocumentSession _documentSession;
    private IEventStore _eventStore;

    public RegisterProductCommandHandlerTests()
    {
        _documentSession = A.Fake<IDocumentSession>();
        _eventPublisher = A.Fake<IEventPublisher>();
        _eventStore = A.Fake<IEventStore>();
        
        A.CallTo(() => _documentSession.Events).Returns(_eventStore);
        A.CallTo(() => _eventStore.StartStream<Product>(A<Guid>.Ignored, A<IEnumerable<object>>.Ignored))
            .Returns(A.Fake<StreamAction>());
        
        _commandHandler = new RegisterProductCommandHandler(_documentSession, _eventPublisher);
    }
    
    [Fact]
    public async Task CanExecuteCommands()
    {
        var command = new RegisterProductCommand(
            "Test coffee", 
            "some description");
        
        var response = await _commandHandler.Handle(command);

        A.CallTo(() => _eventStore.StartStream<Product>(A<Guid>.Ignored, A<IEnumerable<object>>.Ignored)).MustHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<object>>.Ignored)).MustHaveHappened();

        response.Should().NotBeNull();
    }
}