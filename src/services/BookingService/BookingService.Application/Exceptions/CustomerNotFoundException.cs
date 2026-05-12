namespace BookingService.Application.Exceptions;

/// <summary>
/// Thrown when a booking create is attempted for a customer id that CustomerService does not know.
/// </summary>
public sealed class CustomerNotFoundException : Exception
{
    public Guid CustomerId { get; }

    public CustomerNotFoundException(Guid customerId)
        : base($"Customer '{customerId}' was not found in CustomerService.")
    {
        CustomerId = customerId;
    }
}
