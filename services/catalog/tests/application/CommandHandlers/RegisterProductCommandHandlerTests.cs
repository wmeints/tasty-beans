using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using TastyBeans.Catalog.Application.CommandHandlers;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;
using Xunit;

namespace TastyBeans.Catalog.Application.Tests.CommandHandlers;

public class RegisterProductCommandHandlerTests
{
    private IProductRepository _productRepository;
    private IEventPublisher _eventPublisher;
    private RegisterProductCommandHandler _commandHandler;

    public RegisterProductCommandHandlerTests()
    {
        _productRepository = A.Fake<IProductRepository>();
        _eventPublisher = A.Fake<IEventPublisher>();
        
        _commandHandler = new RegisterProductCommandHandler(_productRepository, _eventPublisher);
        
        A.CallTo(() => _productRepository.InsertAsync(A<Product>.Ignored)).Returns(1);
    }
    
    [Fact]
    public async Task CanExecuteCommands()
    {
        var command = new RegisterProductCommand(
            "Test coffee", 
            "some description");
        
        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _productRepository.InsertAsync(A<Product>.Ignored)).MustHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<object>>.Ignored)).MustHaveHappened();

        response.Should().NotBeNull();
    }
}