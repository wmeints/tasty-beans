using TastyBeans.Registration.Api.Application.Commands;
using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Infrastructure.Clients.Payments;

public record RegisterPaymentMethodRequest(Guid CustomerId, string CardHolderName, string CardNumber,
    string SecurityCode, string ExpirationDate, CardType CardType);