using FluentValidation.Results;

namespace RecommendCoffee.Shared.Domain;

public static class ValidationResultExtensions {
    public static IEnumerable<ValidationError> GetValidationErrors(this ValidationResult results)
    {
        return results.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage));
    }
}