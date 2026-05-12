namespace CustomerService.Domain.Entities;

/// <summary>
/// Represents a customer profile used across booking and other services.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier of the customer (referenced by other services, e.g. bookings).
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Customer display name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Primary contact email (unique in this service).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the customer record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
