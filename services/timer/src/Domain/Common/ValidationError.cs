namespace RecommendCoffee.Timer.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);