using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Enums;

namespace EventBookingApp.Application.Interfaces;

public interface IEventService
{
    Task<EventDto> CreateEventAsync(CreateEventDto dto, string organizerId);
    Task<IEnumerable<EventDto>> GetAllEventsAsync();
    Task<EventDto?> GetEventByIdAsync(Guid id);
    Task<EventDto?> UpdateEventAsync(Guid id, UpdateEventDto dto, Guid userId);
    Task<DeleteEventResult> DeleteEventAsync(Guid id, Guid userId);
    Task<IEnumerable<EventDto>> GetEventsByOrganizerAsync(string organizerId);
}
