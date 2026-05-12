using BookingService.Domain.Enums;

namespace BookingService.Application.DTOs.Requests;

public class SaveBookingRequest
{
    public Guid? Id { get; set; }

    public Guid CustomerId { get; set; }

    public string SpaceName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}