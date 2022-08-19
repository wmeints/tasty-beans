using Marten.Schema;

namespace TastyBeans.Profile.Api.Application.ReadModels;

public record CancelledSubscription
{
    public CancelledSubscription(Guid customerId, DateTime endDate)
    {
        CustomerId = customerId;
        EndDate = endDate;
    }

    [Identity]
    public Guid CustomerId { get; }
    
    public DateTime EndDate { get; }
}