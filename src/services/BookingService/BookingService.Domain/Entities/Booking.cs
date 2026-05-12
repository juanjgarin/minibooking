using BookingService.Domain.Enums;

namespace BookingService.Domain.Entities;

/// <summary>
/// Represents a reservation of a named space for a customer between two dates.
/// </summary>
public class Booking
{
    /// <summary>
    /// Unique identifier of the booking.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the customer who owns the booking.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Human-readable name of the reserved space (room, desk, etc.).
    /// </summary>
    public string SpaceName { get; set; } = string.Empty;

    /// <summary>
    /// Start of the reservation interval (inclusive).
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End of the reservation interval (exclusive or inclusive depending on product rules; must be after <see cref="StartDate"/>).
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Current lifecycle state of the booking.
    /// </summary>
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// UTC timestamp when the booking record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updates <see cref="Status"/> to the given value.
    /// </summary>
    /// <param name="status">New status.</param>
    public void ChangeStatus(BookingStatus status)
    {
        Status = status;
    }
}
