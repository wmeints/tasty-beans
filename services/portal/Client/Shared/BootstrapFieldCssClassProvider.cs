using Microsoft.AspNetCore.Components.Forms;

namespace TastyBeans.Portal.Client.Shared;

public class BootstrapFieldCssClassProvider: FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var invalid = editContext.GetValidationMessages(fieldIdentifier).Any();
        return invalid ? "is-invalid" : "";
    }
}