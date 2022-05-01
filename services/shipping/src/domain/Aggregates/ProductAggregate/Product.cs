using TastyBeans.Shared.Domain;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate.Validators;

namespace TastyBeans.Shipping.Domain.Aggregates.ProductAggregate;

public class Product
{
    
#nullable disable

    private Product()
    {
    }

#nullable enable

    private Product(Guid id, string name)
    {
        this.Name = name;
        this.Id = id;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public static RegisterProductCommandResponse Register(RegisterProductCommand cmd)
    {
        using var activity = Activities.RegisterProduct(cmd.Id);

        var validator = new RegisterProductCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new RegisterProductCommandResponse(null, validationResult.GetValidationErrors());
        }

        var product = new Product(cmd.Id, cmd.Name);

        return new RegisterProductCommandResponse(product, Enumerable.Empty<ValidationError>());
    }

    public UpdateProductCommandResponse Update(UpdateProductCommand cmd)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new UpdateProductCommandResponse(validationResult.GetValidationErrors());
        }

        Name = cmd.Name;

        return new UpdateProductCommandResponse(
            Enumerable.Empty<ValidationError>());
    }
}