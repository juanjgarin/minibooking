using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Responses;

namespace CustomerService.Application.Interfaces;

public interface ICustomerAppService
{
    Task<List<CustomerResponse>> GetAllAsync();

    Task<CustomerResponse?> GetByIdAsync(Guid id);

    Task<CustomerResponse> CreateOrUpdateAsync(SaveCustomerRequest request);

    Task<bool> DeleteAsync(Guid id);
}
