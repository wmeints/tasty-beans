namespace TastyBeans.Profile.Api.Application.ReadModels;

public record SubscriptionHistoryItem(Guid Id, Guid CustomerId, DateTime StartDate, DateTime EndDate);