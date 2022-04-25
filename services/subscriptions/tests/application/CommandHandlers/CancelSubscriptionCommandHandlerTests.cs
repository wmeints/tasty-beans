using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using RecommendCoffee.Shared.Application;
using RecommendCoffee.Shared.Domain;
using RecommendCoffee.Subscriptions.Application.CommandHandlers;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using Xunit;

namespace RecommendCoffee.Subscriptions.Application.Tests.CommandHandlers;

public class CancelSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task CanExecuteCommand()
    {
        var subscriptionRepository = A.Fake<ISubscriptionRepository>();
        var eventPublisher = A.Fake<IEventPublisher>();
        var commandHandler = new CancelSubscriptionCommandHandler(subscriptionRepository, eventPublisher);

        var subscription = new Subscription(
            Guid.NewGuid(), 
            DateTime.UtcNow, 
            ShippingFrequency.Weekly,
            SubscriptionKind.OneYear);
        
        A.CallTo(() => subscriptionRepository.FindByCustomerIdAsync(A<Guid>.Ignored)).Returns(subscription);
        
        var command = new CancelSubscriptionCommand(subscription.Id);

        var response = await commandHandler.ExecuteAsync(command);

        response.Should().NotBeNull();

        A.CallTo(() => subscriptionRepository.UpdateAsync(A<Subscription>.Ignored)).MustHaveHappened();
        A.CallTo(() => eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
    }
}