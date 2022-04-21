namespace RecommendCoffee.Templates.Microservice.Domain.Common;

public record PagedResult<T>(IEnumerable<T> Items, int PageIndex, int PageSize, long TotalItems);