using BookingService.Domain.Enums;

namespace BookingService.Application.DTOs.Requests;

public class ChangeStatusRequest
{
    public Guid Id { get; set; }

    public BookingStatus Status { get; set; }
}