using Marten;
using TastyBeans.Profile.Api.Application.Queries;
using TastyBeans.Profile.Api.Application.ReadModels;

namespace TastyBeans.Profile.Api.Application.QueryHandlers;

public class GetSubscriptionHistoryQueryHandler
{
    public static async Task<IEnumerable<SubscriptionHistoryItem>?> Handle(
        GetSubscriptionHistoryQuery message,
        IDocumentSession session)
    {
        if (!await session.Query<CustomerInfo>().AnyAsync(x => x.Id == message.CustomerId))
        {
            return null;
        }
        
        return await session
            .Query<SubscriptionHistoryItem>()
            .Where(x=>x.CustomerId == message.CustomerId)
            .ToListAsync();
    }
}