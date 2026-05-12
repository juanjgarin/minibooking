using AutoMapper;
using BookingService.Application.DTOs.Requests;
using BookingService.Application.DTOs.Responses;
using BookingService.Application.Exceptions;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using BookingService.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BookingService.Application.Services;

public class BookingAppService : IBookingAppService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingAppService> _logger;

    public BookingAppService(
        IBookingRepository bookingRepository,
        ICustomerServiceClient customerServiceClient,
        IMapper mapper,
        ILogger<BookingAppService> logger)
    {
        _bookingRepository = bookingRepository;
        _customerServiceClient = customerServiceClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<BookingResponse>> GetAllAsync()
    {
        _logger.LogInformation("Loading all bookings.");
        var bookings = await _bookingRepository.GetAllAsync();

        return _mapper.Map<List<BookingResponse>>(bookings);
    }

    public async Task<BookingResponse?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Loading booking {BookingId}.", id);
        var booking = await _bookingRepository.GetByIdAsync(id);

        return booking is null ? null : _mapper.Map<BookingResponse>(booking);
    }

    public async Task<BookingResponse> CreateOrUpdateAsync(SaveBookingRequest request)
    {
        Booking? booking = null;

        if (request.Id.HasValue && request.Id.Value != Guid.Empty)
            booking = await _bookingRepository.GetByIdAsync(request.Id.Value);

        if (booking is null)
        {
            _logger.LogInformation(
                "Creating booking for customer {CustomerId}, space {SpaceName}.",
                request.CustomerId,
                request.SpaceName);

            if (!await _customerServiceClient.CustomerExistsAsync(request.CustomerId))
            {
                _logger.LogWarning(
                    "Rejecting booking create: customer {CustomerId} not found in CustomerService.",
                    request.CustomerId);
                throw new CustomerNotFoundException(request.CustomerId);
            }

            booking = _mapper.Map<Booking>(request);

            booking.Id = Guid.NewGuid();
            booking.CreatedAt = DateTime.UtcNow;

            await _bookingRepository.AddAsync(booking);
        }
        else
        {
            _logger.LogInformation("Updating booking {BookingId}.", booking.Id);
            _mapper.Map(request, booking);
        }

        await _bookingRepository.SaveChangesAsync();

        return _mapper.Map<BookingResponse>(booking);
    }

    public async Task<bool> ChangeStatus(Guid id, BookingStatus status)
    {
        _logger.LogInformation("Changing status of booking {BookingId} to {Status}.", id, status);

        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking is null)
        {
            _logger.LogWarning("Booking {BookingId} not found for status change.", id);
            return false;
        }

        booking.ChangeStatus(status);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }
}
