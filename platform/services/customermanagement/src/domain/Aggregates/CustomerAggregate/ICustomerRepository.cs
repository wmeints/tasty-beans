﻿using TastyBeans.Shared.Domain;

namespace TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;

public interface ICustomerRepository
{
    Task<PagedResult<Customer>> FindAllAsync(int pageIndex, int pageSize);
    Task<Customer?> FindById(Guid id);
    Task<int> InsertAsync(Customer customer);
    Task<int> UpdateAsync(Customer customer);
}
