using EventBookingApp.Domain.Entities;

namespace EventBookingApp.Application.DTOs;

public class CreateEventDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<string> TicketCategories { get; set; } = new();
}
