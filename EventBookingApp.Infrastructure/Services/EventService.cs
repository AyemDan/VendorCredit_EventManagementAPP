using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Interfaces;
using EventBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApp.Infrastructure.Services;

// public class EventService : IEventService
// {
//     private readonly AppDbContext _context;

//     public EventService(AppDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<EventDto> CreateEventAsync(CreateEventDto dto, string organizerId)
//     {
//         var evt = new Event
//         {
//             Id = Guid.NewGuid(),
//             Title = dto.Title,
//             Description = dto.Description,
//             Date = dto.Date,
//             Location = dto.Location,
//             OrganizerId = organizerId,
//             TicketCategories =
//                 dto.TicketCategories?.Select(name => new TicketCategory { Name = name }).ToList()
//                 ?? new List<TicketCategory>(),
//         };

//         _context.Events.Add(evt);
//         await _context.SaveChangesAsync();

//         return ToDto(evt);
//     }

//     public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
//     {
//         var events = await _context.Events.ToListAsync();
//         return events.Select(ToDto);
//     }

//     public async Task<EventDto?> GetEventByIdAsync(Guid id)
//     {
//         var evt = await _context.Events.FindAsync(id);
//         return evt is null ? null : ToDto(evt);
//     }

//     public async Task<EventDto?> UpdateEventAsync(Guid id, UpdateEventDto dto)
//     {
//         var evt = await _context.Events.FindAsync(id);
//         if (evt == null)
//             return null;

//         evt.Title = dto.Title ?? evt.Title;
//         evt.Description = dto.Description ?? evt.Description;
//         evt.Date = dto.Date ?? evt.Date;
//         evt.Location = dto.Location ?? evt.Location;

//         if (dto.TicketCategories != null)
//             evt.TicketCategories = dto
//                 .TicketCategories.Select(name => new TicketCategory { Name = name })
//                 .ToList();

//         await _context.SaveChangesAsync();
//         return ToDto(evt);
//     }

//     public async Task<bool> DeleteEventAsync(Guid id)
//     {
//         var evt = await _context.Events.FindAsync(id);
//         if (evt == null)
//             return false;

//         _context.Events.Remove(evt);
//         await _context.SaveChangesAsync();
//         return true;
//     }

//     public async Task<IEnumerable<EventDto>> GetEventsByOrganizerAsync(string organizerId)
//     {
//         var events = await _context.Events.Where(e => e.OrganizerId == organizerId).ToListAsync();
//         return events.Select(ToDto);
//     }

//     private static EventDto ToDto(Event evt) =>
//         new()
//         {
//             Id = evt.Id,
//             Title = evt.Title,
//             Description = evt.Description,
//             Location = evt.Location,
//             Date = evt.Date,
//             OrganizerId = evt.OrganizerId,
//             TicketCategories = evt.TicketCategories.Select(tc => tc.Name).ToList(),
//         };
// }

public class EventService : IEventService
{
    private static readonly List<Event> _events = new();

    public async Task<EventDto> CreateEventAsync(CreateEventDto dto, string organizerId)
    {
        var evt = new Event
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            Location = dto.Location,
            OrganizerId = organizerId,
            TicketCategories =
                dto.TicketCategories?.Select(name => new TicketCategory { Name = name }).ToList()
                ?? new List<TicketCategory>(),
        };

        _events.Add(evt);
        return await Task.FromResult(ToDto(evt));
    }

    public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
    {
        return await Task.FromResult(_events.Select(ToDto));
    }

    public async Task<EventDto?> GetEventByIdAsync(Guid id)
    {
        var evt = _events.FirstOrDefault(e => e.Id == id);
        return await Task.FromResult(evt == null ? null : ToDto(evt));
    }

    public async Task<EventDto?> UpdateEventAsync(Guid id, UpdateEventDto dto)
    {
        var evt = _events.FirstOrDefault(e => e.Id == id);
        if (evt == null)
            return null;

        evt.Title = dto.Title ?? evt.Title;
        evt.Description = dto.Description ?? evt.Description;
        evt.Date = dto.Date ?? evt.Date;
        evt.Location = dto.Location ?? evt.Location;

        if (dto.TicketCategories != null)
            evt.TicketCategories = dto
                .TicketCategories.Select(name => new TicketCategory { Name = name })
                .ToList();

        return await Task.FromResult(ToDto(evt));
    }

    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var evt = _events.FirstOrDefault(e => e.Id == id);
        if (evt == null)
            return false;

        _events.Remove(evt);
        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<EventDto>> GetEventsByOrganizerAsync(string organizerId)
    {
        var events = _events.Where(e => e.OrganizerId == organizerId);
        return await Task.FromResult(events.Select(ToDto));
    }

    private static EventDto ToDto(Event evt) =>
        new()
        {
            Id = evt.Id,
            Title = evt.Title,
            Description = evt.Description,
            Location = evt.Location,
            Date = evt.Date,
            OrganizerId = evt.OrganizerId,
            TicketCategories = evt.TicketCategories.Select(tc => tc.Name).ToList(),
        };
}
