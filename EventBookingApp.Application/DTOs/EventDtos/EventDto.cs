using EventBookingApp.Domain.Entities;

namespace EventBookingApp.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal? Price { get; set; }

    public int Capacity { get; set; }
    public DateTime Date { get; set; }
    public string? OrganizerId { get; set; } = string.Empty;
}
