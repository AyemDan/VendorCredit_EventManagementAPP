using EventBookingApp.Domain.Entities;

namespace EventBookingApp.Application.DTOs;

public class UpdateEventDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime? Date { get; set; }
    public List<string>? TicketCategories { get; set; }
}
