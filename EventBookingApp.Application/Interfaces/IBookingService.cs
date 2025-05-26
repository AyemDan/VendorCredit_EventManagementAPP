using EventBookingApp.Application.DTOs;

namespace EventBookingApp.Application.Interfaces;

public interface IBookingService
{
    Task<BookingDto> CreateBookingAsync(Guid eventId, CreateBookingDto dto, Guid userId);
    Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(Guid userId);
    Task<bool> CancelBookingAsync(Guid bookingId, Guid userId, string? reason = null);
}
