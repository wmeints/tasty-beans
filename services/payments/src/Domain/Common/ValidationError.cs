namespace RecommendCoffee.Payments.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);