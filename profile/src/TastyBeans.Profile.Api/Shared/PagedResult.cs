namespace TastyBeans.Profile.Api.Shared;

public record PagedResult<T>(int PageIndex, int PageSize, long TotalItemCount, IEnumerable<T> Items);