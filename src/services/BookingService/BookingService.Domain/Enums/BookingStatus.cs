namespace BookingService.Domain.Enums;

/// <summary>
/// Lifecycle states for a <see cref="BookingService.Domain.Entities.Booking"/>.
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Booking created and awaiting confirmation.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Booking accepted and active.
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// Booking voided or declined.
    /// </summary>
    Cancelled = 3
}
