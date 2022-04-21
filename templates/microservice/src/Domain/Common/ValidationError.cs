namespace RecommendCoffee.Templates.Microservice.Domain.Common;

public record ValidationError(string PropertyPath, string ErrorMessage);