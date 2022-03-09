using System;
using FluentAssertions;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Validators;
using Xunit;

namespace RecommendCoffee.Payments.Domain.Tests.Aggregates.PaymentMethodAggregate.Validators;

public class RegisterPaymentMethodCommandValidatorTests
{
    [Fact]
    public void Validate_CorrectInput_ReturnsValidResult()
    {
        var cmd = new RegisterPaymentMethodCommand(
            Guid.NewGuid(),
            CardType.Mastercard,
            "5413675197898462",
            "01/22",
            "679", 
            "Test User",
            Guid.NewGuid());

        var validator = new RegisterPaymentMethodCommandValidator();
        var validationResult = validator.Validate(cmd);

        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1/00")]
    [InlineData("/")]
    [InlineData("00000")]
    [InlineData("ABCDE")]
    public void Validate_IncorrectExpirationDate_ReturnsInvalidResult(string expirationDate)
    {
        var cmd = new RegisterPaymentMethodCommand(
            Guid.NewGuid(),
            CardType.Mastercard,
            "5413675197898462",
            expirationDate,
            "679", 
            "Test User",
            Guid.NewGuid());

        var validator = new RegisterPaymentMethodCommandValidator();
        var validationResult = validator.Validate(cmd);

        validationResult.IsValid.Should().BeFalse();
    }
}