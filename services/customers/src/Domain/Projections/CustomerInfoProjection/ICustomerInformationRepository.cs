using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

namespace RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;

public interface ICustomerInformationRepository
{
    Task Insert(CustomerInformation customerInformation);
    Task<FindCustomerQueryResult> FindById(FindCustomerQuery query);
    Task<FindAllCustomersQueryResult> FindAll(FindAllCustomersQuery query);
}