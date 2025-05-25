using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using EventBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApp.Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto, Guid userId)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventId = dto.EventId,
            Quantity = dto.Quantity,
            BookedAt = DateTime.UtcNow,
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            EventId = booking.EventId,
            Quantity = booking.Quantity,
            BookedAt = booking.BookedAt,
        };
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(Guid userId)
    {
        return await _context
            .Bookings.Where(b => b.UserId == userId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                EventId = b.EventId,
                Quantity = b.Quantity,
                BookedAt = b.BookedAt,
            })
            .ToListAsync();
    }

    public async Task<bool> CancelBookingAsync(Guid bookingId, Guid userId, string? reason)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b =>
            b.Id == bookingId && b.UserId == userId
        );

        if (booking == null)
            return false;

        _context.Bookings.Remove(booking);
        booking.IsCancelled = true;
        booking.CancelReason = reason;
        booking.CancelledAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
