using Marten;
using TastyBeans.Profile.Api.Application.Queries;
using TastyBeans.Profile.Api.Application.ReadModels;

namespace TastyBeans.Profile.Api.Application.QueryHandlers;

public class GetCustomerDetailsQueryHandler
{
    public static async Task<CustomerInfo?> Handle(GetCustomerDetailsQuery message, IDocumentSession session)
    {
        return await session
            .Query<CustomerInfo>()
            .SingleOrDefaultAsync(x => x.Id == message.CustomerId);
    }
}