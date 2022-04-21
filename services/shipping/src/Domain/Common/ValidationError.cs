namespace RecommendCoffee.Shipping.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);