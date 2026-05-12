namespace BookingService.Application.DTOs.Responses;

public class BookingResponse
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string SpaceName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}