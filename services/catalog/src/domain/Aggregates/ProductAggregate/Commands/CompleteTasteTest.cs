namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record CompleteTasteTest(Guid ProductId, int RoastLevel, string Taste, IEnumerable<string> FlavorNotes);