namespace RecommendCoffee.Catalog.Application.QueryHandlers;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> Execute(TQuery query);
}
