using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Shared.Application;

public class EventValidationFailedException : Exception
{
    public EventValidationFailedException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
    
    public IEnumerable<ValidationError> Errors { get; }
}