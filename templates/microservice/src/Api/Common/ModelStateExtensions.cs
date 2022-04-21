using Microsoft.AspNetCore.Mvc.ModelBinding;
using RecommendCoffee.Templates.Microservice.Domain.Common;

namespace RecommendCoffee.Templates.Microservice.Api.Common;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.PropertyPath,error.ErrorMessage);
        }
    }
}