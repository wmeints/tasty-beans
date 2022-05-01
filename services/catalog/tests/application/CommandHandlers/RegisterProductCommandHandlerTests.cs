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
            "some description", 
            new List<ProductVariant>
            {
                new ProductVariant(250, 9.95m)
            });
        
        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _productRepository.InsertAsync(A<Product>.Ignored)).MustHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();

        response.Should().NotBeNull();
    }

    [Fact]
    public async Task DoesntPublishEventsWhenResponseIsInvalid()
    {
        var command = new RegisterProductCommand(
            "Test coffee", 
            "some description", 
            new List<ProductVariant>());
        
        var response = await _commandHandler.ExecuteAsync(command);

        A.CallTo(() => _productRepository.InsertAsync(A<Product>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => _eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustNotHaveHappened();

        response.Should().NotBeNull();
    }
}