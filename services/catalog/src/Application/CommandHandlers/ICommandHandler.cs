namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public interface ICommandHandler<TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request);
}

public interface ICommandHandler<TRequest>
{
    Task Execute(TRequest request);
}
