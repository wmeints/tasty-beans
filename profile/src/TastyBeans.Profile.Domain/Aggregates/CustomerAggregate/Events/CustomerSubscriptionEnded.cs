using Jasper.Attributes;

namespace TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

public record CustomerSubscriptionEnded(Guid CustomerId, DateTime StartDate, DateTime EndDate);