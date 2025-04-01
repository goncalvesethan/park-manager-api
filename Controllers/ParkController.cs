using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/parks")]
[Authorize]

public class ParkController : ControllerBase
{
    private readonly ParkManagerContext _context;

    public ParkController(ParkManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Park>>> GetAll()
    {
        var parks = await _context.Parks.Where(p => p.DeletedAt == null).ToListAsync();
        return Ok(parks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Park>> GetById(int id)
    {
        var park = await _context.Parks.FindAsync(id);
        if (park == null) return NotFound("Park not found !");
        
        return Ok(park);
    }

    [HttpPost]
    public async Task<ActionResult<Park>> CreatePark([FromBody]Park request)
    {
        try
        {
            _context.Parks.Add(request);
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
    public async Task<ActionResult<Park>> UpdatePark(int id, [FromBody] Park request)
    {
        try
        {
            var park = await _context.Parks.FindAsync(id);
            park.Name = request.Name;
            park.Location = request.Location;
            park.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(park);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Park>> SofDeletePark(int id)
    {
        try
        {
            var park = await _context.Parks.FindAsync(id);
            
            park.UpdatedAt = DateTime.Now;
            park.DeletedAt = DateTime.Now;
            
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