namespace TastyBeans.Shared.Domain;

public record ValidationError(string PropertyPath, string ErrorMessage);