using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using Action = ParkManagerAPI.Models.Action;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/incidents")]

public class IncidentController : ControllerBase
{
    private readonly ParkManagerContext _context;

    public IncidentController(ParkManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Incident>>> GetAll()
    {
        var incidents = await _context.Incidents.Where(i => i.DeletedAt == null).ToListAsync();
        return Ok(incidents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Incident>> GetById(int id)
    {
        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null) return NotFound("Incident not found !");
        
        return Ok(incident);
    }

    [HttpPost]
    public async Task<ActionResult<Incident>> CreateAction([FromBody] Incident request)
    {
        try
        {
            _context.Incidents.Add(request);
            request.Status = "open";
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

    [HttpPatch("{id}/close")]
    public async Task<ActionResult<Incident>> SetAsClosed(int id)
    {
        try
        {
            var incident = await _context.Incidents.FindAsync(id);

            incident.Status = "closed";
            incident.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(incident);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Incident>> UpdateDevice(int id, [FromBody] Incident request)
    {
        try
        {
            var incident = await _context.Incidents.FindAsync(id);
            
            incident.ReporterId = request.ReporterId;
            incident.DeviceId = request.DeviceId;
            incident.Type = request.Type;
            incident.Status = request.Status;
            incident.Description = request.Description;
            incident.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(incident);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Action>> SofDeletePark(int id)
    {
        try
        {
            var incident = await _context.Incidents.FindAsync(id);
            
            incident.UpdatedAt = DateTime.Now;
            incident.DeletedAt = DateTime.Now;
            
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