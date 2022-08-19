using Jasper.Attributes;

namespace TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

public record CustomerUnsubscribed(Guid CustomerId, DateTime SubscriptionEndDate);
