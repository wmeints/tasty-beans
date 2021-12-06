using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

namespace RecommendCoffee.Customers.Application.QueryHandlers;

public class FindAllCustomersQueryHandler: IQueryHandler<FindAllCustomersQuery, FindAllCustomersQueryResult>
{
    private readonly ICustomerInformationRepository _customerInformationRepository;

    public FindAllCustomersQueryHandler(ICustomerInformationRepository customerInformationRepository)
    {
        _customerInformationRepository = customerInformationRepository;
    }

    public async Task<FindAllCustomersQueryResult> Execute(FindAllCustomersQuery query)
    {
        return await _customerInformationRepository.FindAll(query);
    }
}