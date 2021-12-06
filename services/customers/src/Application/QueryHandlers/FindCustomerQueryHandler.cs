using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

namespace RecommendCoffee.Customers.Application.QueryHandlers;

public class FindCustomerQueryHandler: IQueryHandler<FindCustomerQuery, FindCustomerQueryResult>
{
    private readonly ICustomerInformationRepository _customerInformationRepository;

    public FindCustomerQueryHandler(ICustomerInformationRepository customerInformationRepository)
    {
        _customerInformationRepository = customerInformationRepository;
    }

    public async Task<FindCustomerQueryResult> Execute(FindCustomerQuery query)
    {
        return await _customerInformationRepository.FindById(query);
    }
}