using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace RecommendCoffee.Payments.Infrastructure.Persistence;

public class PaymentMethodRepository: IPaymentMethodRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public PaymentMethodRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<PaymentMethod?> FindByCustomerIdAsync(Guid customerId)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        return await _applicationDbContext.PaymentMethods
            .SingleOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task<int> InsertAsync(PaymentMethod paymentMethod)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        await _applicationDbContext.AddAsync(paymentMethod);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(PaymentMethod paymentMethod)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        _applicationDbContext.Update(paymentMethod);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(PaymentMethod paymentMethod)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        _applicationDbContext.Remove(paymentMethod);
        return await _applicationDbContext.SaveChangesAsync();
    }
}