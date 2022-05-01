namespace TastyBeans.Ratings.Application.IntegrationEvents;

public record ProductRegisteredEvent(
    Guid ProductId,
    string Name,
    string Description,
    IEnumerable<ProductVariant> Variants);