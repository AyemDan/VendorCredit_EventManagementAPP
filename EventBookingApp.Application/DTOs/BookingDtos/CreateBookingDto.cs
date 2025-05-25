using EventBookingApp.Domain.Entities;

namespace EventBookingApp.Application.DTOs;

public class CreateBookingDto
{
    public Guid EventId { get; set; }
    public int Quantity { get; set; }
}
