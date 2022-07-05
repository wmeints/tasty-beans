using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.Commands;

public record DiscontinueProductCommandResponse(IEnumerable<BusinessRuleViolation> Errors)
{
    public bool IsValid => !Errors.Any();
}