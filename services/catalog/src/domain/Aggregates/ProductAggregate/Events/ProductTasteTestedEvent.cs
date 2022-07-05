using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.registered.v1")]
public record ProductTasteTestedEvent(
    Guid ProductId,
    string Taste, 
    IEnumerable<string> FlavorNotes,
    int RoastLevel 
);