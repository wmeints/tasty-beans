namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public interface ICommandHandler<TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request);
}
