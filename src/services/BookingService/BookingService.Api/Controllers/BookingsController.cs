using BookingService.Application.DTOs.Requests;
using BookingService.Application.Exceptions;
using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingAppService _bookingAppService;

    public BookingsController(IBookingAppService bookingAppService)
    {
        _bookingAppService = bookingAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingAppService.GetAllAsync();

        return Ok(bookings);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var booking = await _bookingAppService.GetByIdAsync(id);

        if (booking is null)
            return NotFound();

        return Ok(booking);
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> CreateOrUpdate(
        [FromBody] SaveBookingRequest request)
    {
        try
        {
            var booking = await _bookingAppService.CreateOrUpdateAsync(request);
            return Ok(booking);
        }
        catch (CustomerNotFoundException ex)
        {
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Customer not found");
        }
    }

    [HttpPut("changeStatus")]
    public async Task<IActionResult> Confirm([FromBody] ChangeStatusRequest request)
    {
        var updated = await _bookingAppService.ChangeStatus(request.Id, request.Status);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}