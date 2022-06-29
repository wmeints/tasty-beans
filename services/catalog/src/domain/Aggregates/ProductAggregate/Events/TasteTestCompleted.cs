using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.registered.v1")]
public record TasteTestCompleted(
    Guid ProductId, 
    int RoastLevel, 
    string Taste, 
    IEnumerable<string> FlavorNotes
): IDomainEvent;