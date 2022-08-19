using System.Diagnostics.CodeAnalysis;
using TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate.Events;
using AggregateRoot = TastyBeans.Payments.Domain.Shared.AggregateRoot;

namespace TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate;

public class PaymentMethod : AggregateRoot
{
    public Guid CustomerId { get; private set; }

    [NotNull] public string? CardHolderName { get; private set; }

    [NotNull] public string? CardNumber { get; private set; }

    [NotNull] public string? SecurityCode { get; private set; }

    [NotNull] public string? ExpirationDate { get; private set; }

    public CardType CardType { get; private set; }

    private PaymentMethod()
    {
    }

    public static PaymentMethod Register(Guid id, Guid customerId, string cardHolderName, string cardNumber,
        string securitycode,
        string expirationDate, CardType cardType)
    {
        var paymentMethod = new PaymentMethod();
        
        paymentMethod.Emit(new PaymentMethodRegistered(
            id, customerId, cardHolderName, cardNumber, securitycode,
            expirationDate, cardType));

        return paymentMethod;
    }

    protected override bool TryApplyDomainEvent(object domainEvent)
    {
        switch (domainEvent)
        {
            case PaymentMethodRegistered paymentMethodRegistered:
                Apply(paymentMethodRegistered);
                break;
            default:
                return false;
        }

        return true;
    }

    private void Apply(PaymentMethodRegistered paymentMethodRegistered)
    {
        Id = paymentMethodRegistered.PaymentMethodId;
        CustomerId = paymentMethodRegistered.CustomerId;
        CardHolderName = paymentMethodRegistered.CardHolderName;
        CardNumber = paymentMethodRegistered.CardNumber;
        SecurityCode = paymentMethodRegistered.SecurityCode;
        ExpirationDate = paymentMethodRegistered.ExpirationDate;
        CardType = paymentMethodRegistered.CardType;

        Version++;
    }
}