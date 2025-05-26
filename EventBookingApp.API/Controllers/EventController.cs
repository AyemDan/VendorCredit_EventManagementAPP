using System.Security.Claims;
using EventBookingApp.Application.DTOs;
using EventBookingApp.Application.Enums;
using EventBookingApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateEvent(CreateEventDto dto)
    {
        var organizerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createdEvent = await _eventService.CreateEventAsync(dto, organizerId);

        return Ok(createdEvent);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        var evt = await _eventService.GetEventByIdAsync(id);
        if (evt == null)
            return NotFound();
        return Ok(evt);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var updated = await _eventService.UpdateEventAsync(id, dto, userId);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var result = await _eventService.DeleteEventAsync(id, userId);
        return result switch
        {
            DeleteEventResult.NotFound => NotFound(),
            DeleteEventResult.Unauthorized => Unauthorized(),
            _ => Ok(new { message = "Event deleted successfully" }),
        };
    }

    // [HttpGet("organizer/{organizerId}")]
    // public async Task<IActionResult> GetEventsByOrganizer(string organizerId)
    // {
    //     var events = await _eventService.GetEventsByOrganizerAsync(organizerId);
    //     return Ok(events);
    // }
}
