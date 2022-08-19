namespace TastyBeans.Profile.Api.Application.Commands;

public record UnsubscribeCustomerCommand(Guid CustomerId, DateTime SubscriptionEndDate);
