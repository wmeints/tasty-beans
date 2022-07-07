using System;
using FluentAssertions;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Events;
using Xunit;

namespace RecommendCoffee.Payments.Domain.Tests.Aggregates.PaymentMethodAggregate;

public class PaymentMethodTests
{
    [Fact]
    public void Register_WithValidInput_ReturnsCorrectReply()
    {
        var cmd = new RegisterPaymentMethodCommand(
            Guid.NewGuid(),
            CardType.Mastercard,
            "5413675197898462",
            "01/22",
            "679", 
            "Test User",
            Guid.NewGuid());

        var reply = PaymentMethod.Register(cmd);

        reply.Should().NotBeNull();
        reply.IsValid.Should().BeTrue();
        reply.Events.Should().ContainSingle(x => x is PaymentMethodRegisteredEvent);
    }
}