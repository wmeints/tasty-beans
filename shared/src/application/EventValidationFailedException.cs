using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Application;

public class EventValidationFailedException : Exception
{
    public EventValidationFailedException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
    
    public IEnumerable<ValidationError> Errors { get; }
}