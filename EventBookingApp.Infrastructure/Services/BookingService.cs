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

    public async Task<BookingDto> CreateBookingAsync(
        Guid eventId,
        CreateBookingDto dto,
        Guid userId
    )
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var userWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (userWallet == null)
            throw new Exception("User wallet not found.");

        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (evt == null)
            throw new Exception("Event not found.");

        // Check existing booking for this user & event
        var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b =>
            b.UserId == userId && b.EventId == eventId && !b.IsCancelled
        );

        int totalQuantityRequested = dto.Quantity;
        if (existingBooking != null)
            totalQuantityRequested += existingBooking.Quantity;

        if (evt.Capacity < totalQuantityRequested)
            throw new Exception("Not enough tickets available for this event.");

        decimal totalCost = evt.Price * dto.Quantity;

        if (userWallet.Balance < totalCost)
            throw new Exception("Insufficient funds in wallet.");

        try
        {
            userWallet.Balance -= totalCost;
            _context.Wallets.Update(userWallet);

            if (existingBooking != null)
            {
                existingBooking.Quantity += dto.Quantity;
                _context.Bookings.Update(existingBooking);
            }
            else
            {
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EventId = evt.Id,
                    Quantity = dto.Quantity,
                    BookedAt = DateTime.UtcNow,
                    IsCancelled = false,
                };
                _context.Bookings.Add(booking);
                existingBooking = booking;
            }

            evt.Capacity -= dto.Quantity;
            _context.Events.Update(evt);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new BookingDto
            {
                Id = existingBooking.Id,
                UserId = existingBooking.UserId,
                EventId = existingBooking.EventId,
                EventTitle = evt.Title,
                Quantity = existingBooking.Quantity,
                BookedAt = existingBooking.BookedAt,
                IsCancelled = existingBooking.IsCancelled,
                CancelReason = existingBooking.CancelReason,
                CancelledAt = existingBooking.CancelledAt,
                Event = evt,
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(Guid userId)
    {
        return await _context
            .Bookings.Include(b => b.Event)
            .Where(b => b.UserId == userId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                EventId = b.EventId,
                EventTitle = b.Event.Title,
                Quantity = b.Quantity,
                BookedAt = b.BookedAt,
                IsCancelled = b.IsCancelled,
                CancelReason = b.CancelReason,
                CancelledAt = b.CancelledAt,
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
