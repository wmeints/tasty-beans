namespace RecommendCoffee.Shared.Domain;

public record PagedResult<T>(IEnumerable<T> Items, int PageIndex, int PageSize, long TotalItems);