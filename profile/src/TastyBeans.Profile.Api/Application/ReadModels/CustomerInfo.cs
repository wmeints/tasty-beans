using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.ReadModels;

public record CustomerInfo(
    Guid Id,
    string FirstName,
    string LastName,
    Address ShippingAddress,
    Address InvoiceAddress,
    string EmailAddress, 
    SubscriptionStatus SubscriptionStatus,
    DateTime? SubscriptionStartDate,
    DateTime? SubscriptionEndDate
);