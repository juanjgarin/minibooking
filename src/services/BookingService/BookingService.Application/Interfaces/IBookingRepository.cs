using BookingService.Domain.Entities;

namespace BookingService.Application.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllAsync();

    Task<Booking?> GetByIdAsync(Guid id);

    Task AddAsync(Booking booking);

    Task SaveChangesAsync();
}