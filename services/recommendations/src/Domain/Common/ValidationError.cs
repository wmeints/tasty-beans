namespace RecommendCoffee.Recommendations.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);