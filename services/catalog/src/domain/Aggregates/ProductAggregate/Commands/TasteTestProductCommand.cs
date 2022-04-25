namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record TasteTestProductCommand(Guid ProductId, int RoastLevel, string Taste, IEnumerable<string> FlavorNotes);