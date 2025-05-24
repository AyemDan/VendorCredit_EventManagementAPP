using EventBookingApp.Application.DTOs;

namespace EventBookingApp.Application.Interfaces;

public interface IEventService
{
    Task<EventDto> CreateEventAsync(CreateEventDto dto, string organizerId);
    Task<IEnumerable<EventDto>> GetAllEventsAsync();
    Task<EventDto?> GetEventByIdAsync(Guid id);
    Task<EventDto?> UpdateEventAsync(Guid id, UpdateEventDto dto);
    Task<bool> DeleteEventAsync(Guid id);
    Task<IEnumerable<EventDto>> GetEventsByOrganizerAsync(string organizerId);
}
