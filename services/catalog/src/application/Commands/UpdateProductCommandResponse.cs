using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.Commands;

public record UpdateProductCommandResponse(IEnumerable<BusinessRuleViolation> Errors)
{
    public bool IsValid => !Errors.Any();
}