using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Enums;
using EventBookingApp.Application.Interfaces;
using EventBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApp.Infrastructure.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EventDto> CreateEventAsync(CreateEventDto dto, string organizerId)
    {
        var evt = new Event
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            Location = dto.Location,
            Price = dto.Price,
            Capacity = dto.Capacity,
            OrganizerId = organizerId,
        };

        _context.Events.Add(evt);
        await _context.SaveChangesAsync();

        return ToDto(evt);
    }

    public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
    {
        var events = await _context.Events.ToListAsync();
        return events.Select(ToDto);
    }

    public async Task<EventDto?> GetEventByIdAsync(Guid id)
    {
        var evt = await _context.Events.FindAsync(id);
        return evt is null ? null : ToDto(evt);
    }

    public async Task<EventDto?> UpdateEventAsync(Guid id, UpdateEventDto dto, Guid userId)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt == null)
            return null;

        if (evt.OrganizerId != userId.ToString())
            return null;

        evt.Title = dto.Title ?? evt.Title;
        evt.Description = dto.Description ?? evt.Description;
        evt.Date = dto.Date ?? evt.Date;
        evt.Price = dto.Price ?? evt.Price;
        evt.Capacity = dto.Capacity ?? evt.Capacity;
        evt.Location = dto.Location ?? evt.Location;

        await _context.SaveChangesAsync();
        return ToDto(evt);
    }

    public async Task<DeleteEventResult> DeleteEventAsync(Guid id, Guid userId)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt == null)
            return DeleteEventResult.NotFound;

        if (evt.OrganizerId != userId.ToString())
            return DeleteEventResult.Unauthorized;

        _context.Events.Remove(evt);
        await _context.SaveChangesAsync();
        return DeleteEventResult.Success;
    }

    public async Task<IEnumerable<EventDto>> GetEventsByOrganizerAsync(string organizerId)
    {
        var events = await _context.Events.Where(e => e.OrganizerId == organizerId).ToListAsync();
        return events.Select(ToDto);
    }

    private static EventDto ToDto(Event evt) =>
        new()
        {
            Id = evt.Id,
            Title = evt.Title,
            Description = evt.Description,
            Location = evt.Location,
            Date = evt.Date,
            Price = evt.Price,
            Capacity = evt.Capacity,
            OrganizerId = evt.OrganizerId,
        };
}
