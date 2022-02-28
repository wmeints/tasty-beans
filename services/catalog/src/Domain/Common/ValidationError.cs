namespace RecommendCoffee.Catalog.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);