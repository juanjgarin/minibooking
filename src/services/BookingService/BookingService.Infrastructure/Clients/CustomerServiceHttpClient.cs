using System.Net;
using BookingService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookingService.Infrastructure.Clients;

public class CustomerServiceHttpClient : ICustomerServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomerServiceHttpClient> _logger;

    public CustomerServiceHttpClient(
        HttpClient httpClient,
        ILogger<CustomerServiceHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            $"api/customers/{customerId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogDebug("CustomerService returned 404 for customer {CustomerId}.", customerId);
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}
