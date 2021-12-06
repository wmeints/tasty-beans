namespace RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

public record FindAllCustomersQueryResult(
    IEnumerable<CustomerInformation> Items, 
    int PageIndex, 
    int PageSize,
    long TotalItemCount);