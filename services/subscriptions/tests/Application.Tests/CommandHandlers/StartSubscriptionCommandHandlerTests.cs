using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Subscriptions.Application.CommandHandlers;
using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using RecommendCoffee.Subscriptions.Domain.Common;
using Xunit;

namespace RecommendCoffee.Subscriptions.Application.Tests.CommandHandlers;

public class StartSubscriptionCommandHandlerTests
{
    [Fact]
    public async Task CanExecuteCommand()
    {
        var subscriptionRepository = A.Fake<ISubscriptionRepository>();
        var eventPublisher = A.Fake<IEventPublisher>();
        var logger = A.Fake<ILogger<StartSubscriptionCommandHandler>>();

        var commandHandler = new StartSubscriptionCommandHandler(eventPublisher, subscriptionRepository, logger);

        A.CallTo(() => subscriptionRepository.FindByCustomerIdAsync(A<Guid>.Ignored)).Returns((Subscription?)null);

        var command = new StartSubscriptionCommand(
            Guid.NewGuid(),
            SubscriptionKind.OneYear,
            ShippingFrequency.Monthly);

        var response = await commandHandler.ExecuteAsync(command);

        response.Should().NotBeNull();

        A.CallTo(() => subscriptionRepository.InsertAsync(A<Subscription>.Ignored)).MustHaveHappened();
        A.CallTo(() => eventPublisher.PublishEventsAsync(A<IEnumerable<IDomainEvent>>.Ignored)).MustHaveHappened();
    }
}