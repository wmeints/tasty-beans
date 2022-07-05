using Microsoft.AspNetCore.Mvc.ModelBinding;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Api.Common;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.PropertyPath, error.ErrorMessage);
        }
    }

    public static void AddValidationErrors(this ModelStateDictionary modelState,
        IEnumerable<BusinessRuleViolation> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(String.Empty, error.ErrorMessage);
        }
    }
}