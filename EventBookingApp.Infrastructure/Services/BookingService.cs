using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using EventBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApp.Infrastructure.Services;

// public class BookingService : IBookingService
// {
//     private readonly AppDbContext _context;

//     public BookingService(AppDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto, string userId)
//     {
//         var booking = new Booking
//         {
//             Id = Guid.NewGuid(),
//             UserId = userId,
//             EventId = dto.EventId,
//             TicketCategoryId = dto.TicketCategoryId,
//             Quantity = dto.Quantity,
//             BookedAt = DateTime.UtcNow,
//         };

//         _context.Bookings.Add(booking);
//         await _context.SaveChangesAsync();

//         return new BookingDto
//         {
//             Id = booking.Id,
//             UserId = booking.UserId,
//             EventId = booking.EventId,
//             TicketCategoryId = booking.TicketCategoryId,
//             Quantity = booking.Quantity,
//             BookedAt = booking.BookedAt,
//         };
//     }

//     public async Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(string userId)
//     {
//         return await _context
//             .Bookings.Where(b => b.UserId == userId)
//             .Select(b => new BookingDto
//             {
//                 Id = b.Id,
//                 UserId = b.UserId,
//                 EventId = b.EventId,
//                 TicketCategoryId = b.TicketCategoryId,
//                 Quantity = b.Quantity,
//                 BookedAt = b.BookedAt,
//             })
//             .ToListAsync();
//     }

//     public async Task<bool> CancelBookingAsync(Guid bookingId, string userId, string? reason)
//     {
//         var booking = await _context.Bookings.FirstOrDefaultAsync(b =>
//             b.Id == bookingId && b.UserId == userId
//         );

//         if (booking == null)
//             return false;

//         _context.Bookings.Remove(booking);
//         booking.IsCancelled = true;
//         booking.CancelReason = reason;
//         booking.CancelledAt = DateTime.UtcNow;

//         await _context.SaveChangesAsync();
//         return true;
//     }
// }

public class BookingService : IBookingService
{
    private readonly List<Booking> _bookings = new();

    public Task<BookingDto> CreateBookingAsync(CreateBookingDto dto, string userId)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventId = dto.EventId,
            TicketCategoryId = dto.TicketCategoryId,
            Quantity = dto.Quantity,
            BookedAt = DateTime.UtcNow,
        };

        _bookings.Add(booking);

        var result = new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            EventId = booking.EventId,
            TicketCategoryId = booking.TicketCategoryId,
            Quantity = booking.Quantity,
            BookedAt = booking.BookedAt,
        };

        return Task.FromResult(result);
    }

    public Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(string userId)
    {
        var bookings = _bookings
            .Where(b => b.UserId == userId && !b.IsCancelled)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                EventId = b.EventId,
                TicketCategoryId = b.TicketCategoryId,
                Quantity = b.Quantity,
                BookedAt = b.BookedAt,
            });

        return Task.FromResult(bookings);
    }

    public Task<bool> CancelBookingAsync(Guid bookingId, string userId, string? reason)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null)
            return Task.FromResult(false);

        booking.IsCancelled = true;
        booking.CancelReason = reason;
        booking.CancelledAt = DateTime.UtcNow;

        return Task.FromResult(true);
    }
}
