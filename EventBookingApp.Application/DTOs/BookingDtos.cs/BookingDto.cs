using EventBookingApp.Domain.Entities;

namespace EventBookingApp.Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public Guid TicketCategoryId { get; set; }
    public string TicketCategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime BookedAt { get; set; }
    public bool IsCancelled { get; set; } = false;
    public string? CancelReason { get; set; }
    public DateTime? CancelledAt { get; set; }

    // Navigation properties
    public Event Event { get; set; } = new();
    public TicketCategory TicketCategory { get; set; } = new();
}
