namespace RecommendCoffee.CustomerManagement.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);