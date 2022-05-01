namespace TastyBeans.Shipping.Application.IntegrationEvents;

public record ProductUpdatedEvent(
    Guid ProductId, 
    string Name, 
    string Description, 
    IEnumerable<ProductVariant> Variants);