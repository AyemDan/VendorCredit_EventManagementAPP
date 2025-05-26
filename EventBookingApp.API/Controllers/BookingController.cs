using System.Security.Claims;
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

    [HttpPost("{eventId}")]
    public async Task<IActionResult> Book(Guid eventId, CreateBookingDto dto)
    {
        var userIdClaim = User
            .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        if (!Guid.TryParse(userIdClaim, out var userGuid))
            return BadRequest("Invalid user ID");
        var booking = await _bookingService.CreateBookingAsync(eventId, dto, userGuid);
        return Ok(booking);
    }

    [HttpGet("my-bookings")]
    public async Task<IActionResult> MyBookings()
    {
        var userIdClaim = User
            .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        if (!Guid.TryParse(userIdClaim, out var userGuid))
            return BadRequest("Invalid user ID");

        var bookings = await _bookingService.GetBookingsByUserAsync(userGuid);
        return Ok(bookings);
    }

    [HttpDelete("{bookingId}")]
    public async Task<IActionResult> CancelBooking(Guid bookingId)
    {
        var userIdClaim = User
            .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        if (!Guid.TryParse(userIdClaim, out var userGuid))
            return BadRequest("Invalid user ID");

        var success = await _bookingService.CancelBookingAsync(bookingId, userGuid);
        if (!success)
            return NotFound();

        return Ok(new { message = "Booking cancelled successfully" });
    }
}

