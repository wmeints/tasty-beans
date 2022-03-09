using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Events;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Validators;
using RecommendCoffee.Payments.Domain.Common;

namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;

public class PaymentMethod
{
    private PaymentMethod()
    {
    }

    public PaymentMethod(
        Guid id, 
        Guid customerId, 
        string cardHolderName,
        string cardNumber,
        string securityCode, 
        string expirationDate,
        CardType type)
    {
        Id = id;
        CustomerId = customerId;
        CardHolderName = cardHolderName;
        CardNumber = cardNumber;
        SecurityCode = securityCode;
        ExpirationDate = expirationDate;
        Type = type;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CardHolderName { get; private set; }
    public string CardNumber { get; private set; }
    public string SecurityCode { get; private set; }
    public string ExpirationDate { get; private set; }
    public CardType Type { get; private set; }

    public static RegisterPaymentMethodReply Register(RegisterPaymentMethodCommand cmd)
    {
        var validator = new RegisterPaymentMethodCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();

            return new RegisterPaymentMethodReply(null, errors, Enumerable.Empty<IDomainEvent>());
        }

        var paymentMethod = new PaymentMethod(
            cmd.Id, cmd.CustomerId, cmd.CardHolderName, cmd.CardNumber,
            cmd.SecurityCode, cmd.ExpirationDate, cmd.CardType);

        var domainEvents = new IDomainEvent[]
        {
            new PaymentMethodRegisteredEvent(
                cmd.Id, 
                cmd.CardType,
                cmd.CardNumber,
                cmd.ExpirationDate,
                cmd.SecurityCode,
                cmd.CardHolderName,
                cmd.CustomerId)
        };

        return new RegisterPaymentMethodReply(
            paymentMethod, 
            Enumerable.Empty<ValidationError>(), 
            domainEvents);
    }
}