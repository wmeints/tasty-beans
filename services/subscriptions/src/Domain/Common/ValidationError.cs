namespace RecommendCoffee.Subscriptions.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);