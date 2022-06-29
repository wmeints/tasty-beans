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

public class RegisterProductCommandHandlerTests
{
    private IEventStore _eventStore;
    private IEventPublisher _eventPublisher;
    private RegisterProductCommandHandler _commandHandler;

    public RegisterProductCommandHandlerTests()
    {
        _eventStore = A.Fake<IEventStore>();
        _eventPublisher = A.Fake<IEventPublisher>();
        
        _commandHandler = new RegisterProductCommandHandler(_eventStore, _eventPublisher);
    }
    
    [Fact]
    public async Task CanExecuteCommands()
    {
        var command = new Register(
            Guid.NewGuid(),
            "Test coffee", 
            "some description");
        
        await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _eventStore.AppendAsync(A<Guid>.Ignored, A<long>.Ignored, A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
    }
}