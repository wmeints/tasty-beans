namespace RecommendCoffee.Ratings.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);