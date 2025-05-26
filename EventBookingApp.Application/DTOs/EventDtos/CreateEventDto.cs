using EventBookingApp.Domain.Entities;

namespace EventBookingApp.Application.DTOs;

public class CreateEventDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }

    public int Capacity { get; set; }
}
