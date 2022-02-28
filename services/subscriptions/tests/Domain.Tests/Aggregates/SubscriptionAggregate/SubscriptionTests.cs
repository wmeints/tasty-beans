using System;
using System.Net;
using FluentAssertions;
using NodaTime;
using NodaTime.Extensions;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;
using Xunit;

namespace RecommendCoffee.Subscriptions.Domain.Tests.Aggregates.SubscriptionAggregate;

public class SubscriptionTests
{
    [Fact]
    public void CanStartSubscription()
    {
        var command = new StartSubscriptionCommand(Guid.NewGuid(), SubscriptionKind.OneYear, ShippingFrequency.Monthly);
        var reply = Subscription.Start(command);

        reply.Subscription.Should().NotBeNull();
        reply.Subscription.Kind.Should().Be(command.Kind);
        reply.Subscription.ShippingFrequency.Should().Be(command.ShippingFrequency);
        reply.Subscription.EndDate.Should().BeNull();

        reply.Errors.Should().BeEmpty();
        reply.Events.Should().ContainSingle(x => x is SubscriptionStartedEvent);
    }

    [Fact]
    public void CanCancelSubscription()
    {
        var subscription = new Subscription(
            Guid.NewGuid(), 
            DateTime.UtcNow, 
            ShippingFrequency.Monthly,
            SubscriptionKind.OneYear);
        
        var command = new CancelSubscriptionCommand(subscription.Id);
        var response = subscription.Cancel(command);

        response.Errors.Should().BeEmpty();
        response.Events.Should().ContainSingle(x => x is SubscriptionCancelledEvent);

        var currentDate = DateTime.UtcNow.ToLocalDateTime().Date;
        var endOfMonth = DateAdjusters.EndOfMonth(currentDate);

        subscription.EndDate.Should().Be(endOfMonth.ToDateTimeUnspecified());
    }

    [Fact]
    public void CanResubscribe()
    {
        var subscription = new Subscription(
            Guid.NewGuid(), 
            DateTime.UtcNow, 
            ShippingFrequency.Monthly,
            SubscriptionKind.OneYear);
        
        subscription.Cancel(new CancelSubscriptionCommand(subscription.Id));
        subscription.Resubscribe(new StartSubscriptionCommand(
            subscription.Id, 
            SubscriptionKind.OneYear,
            ShippingFrequency.Monthly));

        subscription.EndDate.Should().BeNull();
        subscription.IsActive.Should().BeTrue();
    }
}