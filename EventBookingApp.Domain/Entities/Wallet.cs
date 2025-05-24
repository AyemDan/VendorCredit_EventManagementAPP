namespace EventBookingApp.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal Balance { get; set; } = 0;

        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
