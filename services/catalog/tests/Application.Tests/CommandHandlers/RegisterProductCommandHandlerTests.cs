using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using RecommendCoffee.Catalog.Application.CommandHandlers;
using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Common;
using Xunit;

namespace RecommendCoffee.Catalog.Application.Tests.CommandHandlers;

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