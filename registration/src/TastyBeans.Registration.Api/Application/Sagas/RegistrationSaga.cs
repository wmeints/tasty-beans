using System.Diagnostics.CodeAnalysis;
using Jasper;
using TastyBeans.Registration.Api.Application.Commands;
using TastyBeans.Registration.Api.Application.IntegrationEvents;
using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Application.Sagas;

public class RegistrationSaga : Saga
{
    public Guid Id { get; set; }

    [NotNull] public CustomerInfo? Customer { get; set; }

    [NotNull] public PaymentMethodInfo? PaymentMethod { get; set; }

    public async Task Start(StartRegistrationCommand message, ILogger<RegistrationSaga> logger, IProfileService profileService)
    {
        Id = message.CustomerId;
        PaymentMethod = message.PaymentMethod;
        Customer = message.Customer;

        logger.LogInformation("Starting registration saga for customer {CustomerId}", Id);
        
        await profileService.RegisterCustomer(new RegisterCustomerRequest(
            message.CustomerId,
            Customer.FirstName,
            Customer.LastName,
            Customer.ShippingAddress,
            Customer.ShippingAddress,
            Customer.EmailAddress));
    }

    public async Task Handle(CustomerRegisteredIntegrationEvent message, ILogger<RegistrationSaga> logger, IPaymentsService paymentsService)
    {
        logger.LogInformation("Customer is registered in the profile service");
        logger.LogInformation("Registering payment method");
        
        await paymentsService.RegisterPaymentMethod(new RegisterPaymentMethodRequest(
            message.CustomerId,
            PaymentMethod.CardHolderName,
            PaymentMethod.CardNumber,
            PaymentMethod.ExpirationDate,
            PaymentMethod.SecurityCode,
            PaymentMethod.CardType));
    }

    public void Handle(PaymentMethodRegisteredIntegrationEvent message, ILogger<RegistrationSaga> logger)
    {
        logger.LogInformation("Payment method is registered");
        logger.LogInformation("Registration completed");
        
        MarkCompleted();
    }
}