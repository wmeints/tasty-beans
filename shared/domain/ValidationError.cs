namespace RecommendCoffee.Shared.Domain;

public record ValidationError(string PropertyPath, string ErrorMessage);