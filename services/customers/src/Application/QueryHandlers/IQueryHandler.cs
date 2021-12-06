namespace RecommendCoffee.Customers.Application.QueryHandlers;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> Execute(TQuery query);
}
