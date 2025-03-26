using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using Action = ParkManagerAPI.Models.Action;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/actions")]

public class ActionController : ControllerBase
{
    private readonly ParkManagerContext _context;

    public ActionController(ParkManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Action>>> GetAll()
    {
        var actions = await _context.Actions.Where(p => p.DeletedAt == null).ToListAsync();
        return Ok(actions);
    }
    
    [HttpGet("mac/{macAddress}")]
    public async Task<ActionResult<Action>> GetDeviceAction(string macAddress)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstAsync();
            var action = await _context.Actions.Where(a => a.Status == "pending").Where(a => a.DeviceId == device.Id).FirstOrDefaultAsync();
            
            return Ok(action);
        }
        catch (Exception e)
        {
            return NotFound("Action not found !");
        }
        
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Action>> GetById(int id)
    {
        var action = await _context.Actions.FindAsync(id);
        if (action == null) return NotFound("Action not found !");
        
        return Ok(action);
    }

    [HttpPost]
    public async Task<ActionResult<Action>> CreateAction([FromBody]Action request)
    {
        try
        {
            _context.Actions.Add(request);
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

    [HttpPatch("mac/{macAddress}")]
    public async Task<ActionResult<Action>> SetActionAsDone(string macAddress)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstAsync();
            var action = await _context.Actions.Where(a => a.Status == "pending").Where(a => a.DeviceId == device.Id)
                .FirstAsync();

            action.Status = "done";
            action.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(action);
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
            var action = await _context.Actions.FindAsync(id);
            
            action.UpdatedAt = DateTime.Now;
            action.DeletedAt = DateTime.Now;
            
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