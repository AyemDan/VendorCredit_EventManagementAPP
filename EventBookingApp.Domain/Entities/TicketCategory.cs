namespace EventBookingApp.Domain.Entities
{
    public class TicketCategory
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }
    }
}
