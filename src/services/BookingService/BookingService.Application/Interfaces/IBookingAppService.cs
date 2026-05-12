using BookingService.Application.DTOs.Requests;
using BookingService.Application.DTOs.Responses;
using BookingService.Domain.Enums;

namespace BookingService.Application.Interfaces;

public interface IBookingAppService
{
    Task<List<BookingResponse>> GetAllAsync();

    Task<BookingResponse?> GetByIdAsync(Guid id);

    Task<BookingResponse> CreateOrUpdateAsync(SaveBookingRequest request);

    Task<bool> ChangeStatus(Guid id, BookingStatus status);
}
