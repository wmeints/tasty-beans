using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Validators;
using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;

public class Product
{
    public Product(Guid id, string name, bool discontinued)
    {
        Id = id;
        Name = name;
        Discontinued = discontinued;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool Discontinued { get; private set; }

    public static RegisterProductCommandResponse Register(RegisterProductCommand cmd)
    {
        var validator = new RegisterProductCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new RegisterProductCommandResponse(null, validationResult.GetValidationErrors());
        }

        var product = new Product(cmd.Id, cmd.Name, false);

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

    public DiscontinueProductCommandResponse Discontinue(DiscontinueProductCommand cmd)
    {
        Discontinued = true;
        return new DiscontinueProductCommandResponse();
    }
}