using AutoMapper;
using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.DTOs.Responses;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CustomerService.Application.Services;

public class CustomerAppService : ICustomerAppService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerAppService> _logger;

    public CustomerAppService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        ILogger<CustomerAppService> logger)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        _logger.LogInformation("Loading all customers.");
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<List<CustomerResponse>>(customers);
    }

    public async Task<CustomerResponse?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Loading customer {CustomerId}.", id);
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer is null ? null : _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse> CreateOrUpdateAsync(SaveCustomerRequest request)
    {
        Customer? customer = null;

        if (request.Id.HasValue && request.Id.Value != Guid.Empty)
            customer = await _customerRepository.GetByIdAsync(request.Id.Value);

        if (customer is null)
        {
            _logger.LogInformation("Creating customer {Email}.", request.Email);
            customer = _mapper.Map<Customer>(request);
            customer.Id = Guid.NewGuid();
            customer.CreatedAt = DateTime.UtcNow;
            await _customerRepository.AddAsync(customer);
        }
        else
        {
            _logger.LogInformation("Updating customer {CustomerId}.", customer.Id);
            _mapper.Map(request, customer);
        }

        await _customerRepository.SaveChangesAsync();
        return _mapper.Map<CustomerResponse>(customer);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting customer {CustomerId}.", id);
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null)
        {
            _logger.LogWarning("Customer {CustomerId} not found for delete.", id);
            return false;
        }

        _customerRepository.Remove(customer);
        await _customerRepository.SaveChangesAsync();
        return true;
    }
}
