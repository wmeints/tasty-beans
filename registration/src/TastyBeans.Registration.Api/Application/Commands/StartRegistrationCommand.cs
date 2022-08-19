using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Application.Commands;

public record StartRegistrationCommand(Guid CustomerId, CustomerInfo Customer, PaymentMethodInfo PaymentMethod);