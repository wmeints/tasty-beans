using Marten;
using Marten.Pagination;
using TastyBeans.Profile.Api.Application.Queries;
using TastyBeans.Profile.Api.Application.ReadModels;
using TastyBeans.Profile.Api.Shared;

namespace TastyBeans.Profile.Api.Application.QueryHandlers;

public class GetCustomersQueryHandler
{
    public static async Task<PagedResult<CustomerInfo>> Handle(
        GetCustomersQuery message,
        IDocumentSession documentSession)
    {
        var result = await documentSession.Query<CustomerInfo>().ToPagedListAsync(message.PageIndex, message.PageSize);
        return new PagedResult<CustomerInfo>(message.PageIndex, message.PageSize, result.TotalItemCount, result);
    }
}