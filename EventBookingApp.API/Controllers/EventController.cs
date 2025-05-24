using EventBookingApp.Application.DTOs;
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
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
    {
        var userId = User.FindFirst("id")?.Value;

        var created = await _eventService.CreateEventAsync(dto, userId);
        return CreatedAtAction(nameof(GetEventById), new { id = created.Id }, created);
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
        var updated = await _eventService.UpdateEventAsync(id, dto);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var success = await _eventService.DeleteEventAsync(id);
        if (!success)
            return NotFound();
        return Ok(new { message = "Event deleted successfully" });
    }

    // [HttpGet("organizer/{organizerId}")]
    // public async Task<IActionResult> GetEventsByOrganizer(string organizerId)
    // {
    //     var events = await _eventService.GetEventsByOrganizerAsync(organizerId);
    //     return Ok(events);
    // }
}
