namespace EventBookingApp.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? OrganizerId { get; set; }

        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public List<TicketCategory> TicketCategories { get; set; } = new();
    }
}
