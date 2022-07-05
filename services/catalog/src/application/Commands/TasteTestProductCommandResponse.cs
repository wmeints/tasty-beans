using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.Commands;

public record TasteTestProductCommandResponse(bool ProductExists, IEnumerable<BusinessRuleViolation> Errors)
{
    public bool IsValid => !Errors.Any();
}
