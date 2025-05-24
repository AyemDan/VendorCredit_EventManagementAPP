namespace EventBookingApp.Domain.Entities
{
    public class EventBooking
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid EventId { get; set; }
        public Event? Event { get; set; }

        public DateTime BookedAt { get; set; } = DateTime.UtcNow;

        public decimal AmountPaid { get; set; }
    }
}
