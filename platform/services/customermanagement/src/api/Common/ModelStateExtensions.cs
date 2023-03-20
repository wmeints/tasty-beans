using Microsoft.AspNetCore.Mvc.ModelBinding;
using TastyBeans.Shared.Domain;

namespace TastyBeans.CustomerManagement.Api.Common;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.PropertyPath, error.ErrorMessage);
        }
    }
}