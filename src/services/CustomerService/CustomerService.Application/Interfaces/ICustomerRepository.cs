using CustomerService.Domain.Entities;

namespace CustomerService.Application.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(Guid id);

    Task AddAsync(Customer customer);

    void Remove(Customer customer);

    Task SaveChangesAsync();
}
