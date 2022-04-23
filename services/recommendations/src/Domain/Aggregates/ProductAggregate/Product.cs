using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Validators;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Recommendations.Domain.Common;

namespace RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate;

public class Product
{
    private Product()
    {
        
    }

    private Product(Guid id, string name)
    {
        this.Id = id;
        this.Name = name;
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