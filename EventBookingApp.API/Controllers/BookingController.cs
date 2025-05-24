using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> Book(CreateBookingDto dto)
    {
        var userId = User.Identity?.Name; // Or from claims
        if (userId == null)
            return Unauthorized();

        var booking = await _bookingService.CreateBookingAsync(dto, userId);
        return Ok(booking);
    }

    [HttpGet("my-bookings")]
    public async Task<IActionResult> MyBookings()
    {
        var userId = User.Identity?.Name;
        if (userId == null)
            return Unauthorized();

        var bookings = await _bookingService.GetBookingsByUserAsync(userId);
        return Ok(bookings);
    }

    [HttpDelete("{bookingId}")]
    public async Task<IActionResult> CancelBooking(Guid bookingId)
    {
        var userId = User.Identity?.Name;
        if (userId == null)
            return Unauthorized();

        var success = await _bookingService.CancelBookingAsync(bookingId, userId);
        if (!success)
            return NotFound();

        return Ok(new { message = "Booking cancelled successfully" });
    }
}
