namespace EventBookingApp.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public Guid TicketCategoryId { get; set; }
        public int Quantity { get; set; }
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;

        public Event Event { get; set; }

        public bool IsCancelled { get; set; } = false;
        public string? CancelReason { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
}
