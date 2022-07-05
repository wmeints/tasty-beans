namespace TastyBeans.Catalog.Application.Commands;

public record TasteTestProductCommand(Guid ProductId, int RoastLevel, string Taste, string[] FlavorNotes);