using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/rooms")]
[Authorize]

public class RoomController : ControllerBase
{
    private readonly ParkManagerContext _context;

    public RoomController(ParkManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetAll()
    {
        var rooms = await _context.Rooms.Where(r => r.DeletedAt == null).ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetById(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound("Room not found !");
        
        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> CreateRoom([FromBody]Room request)
    {
        try
        {
            _context.Rooms.Add(request);
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Room>> UpdateRoom(int id, [FromBody] Room request)
    {
        try
        {
            var room = await _context.Rooms.FindAsync(id);
            room.ParkId = request.ParkId;
            room.Name = request.Name;
            room.Type = request.Type;
            room.Capacity = request.Capacity;
            room.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(room);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Room>> SofDeleteRoom(int id)
    {
        try
        {
            var room = await _context.Rooms.FindAsync(id);
            
            room.UpdatedAt = DateTime.Now;
            room.DeletedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
    }
}