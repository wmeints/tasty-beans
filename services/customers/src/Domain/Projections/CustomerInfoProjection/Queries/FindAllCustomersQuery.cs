namespace RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

public record FindAllCustomersQuery(int PageIndex, int PageSize);