namespace TastyBeans.Profile.Api.Application.Commands;

public record SubscribeCustomerCommand(Guid CustomerId, DateTime SubscriptionStartDate);
