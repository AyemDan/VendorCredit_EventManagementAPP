﻿namespace EventBookingApp.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Wallet? Wallet { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
