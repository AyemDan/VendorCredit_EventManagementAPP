namespace EventBookingApp.Application.Interfaces;

using EventBookingApp.Application.DTOs;

public interface IBookingService
{
    Task<BookingDto> CreateBookingAsync(CreateBookingDto dto, string userId);
    Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(string userId);
    Task<bool> CancelBookingAsync(Guid bookingId, string userId, string? reason = null);
}
