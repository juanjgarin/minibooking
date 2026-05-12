namespace BookingService.Application.Interfaces;

/// <summary>
/// Calls CustomerService to verify customer-related rules (e.g. existence before booking).
/// </summary>
public interface ICustomerServiceClient
{
    /// <summary>
    /// Returns true if CustomerService responds 200 for GET /api/customers/{customerId}.
    /// </summary>
    Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default);
}
