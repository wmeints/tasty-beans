namespace TastyBeans.Recommendations.Application.IntegrationEvents;

public record ProductUpdatedEvent(
    Guid ProductId, 
    string Name, 
    string Description, 
    IEnumerable<ProductVariant> Variants);