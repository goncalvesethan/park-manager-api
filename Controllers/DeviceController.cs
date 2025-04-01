using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/devices")]
[Authorize]

public class DeviceController : ControllerBase
{
    private readonly ParkManagerContext _context;

    public DeviceController(ParkManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Device>>> GetAll()
    {
        var devices = await _context.Devices.Where(d => d.DeletedAt == null).ToListAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetById(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null) return NotFound("Device not found !");
        
        return Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<Device>> CreateDevice([FromBody]Device request)
    {
        try
        {
            _context.Devices.Add(request);
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
    public async Task<ActionResult<Device>> UpdateDevice(int id, [FromBody] Device request)
    {
        try
        {
            var device = await _context.Devices.FindAsync(id);
            device.ParkId = request.ParkId;
            device.RoomId = request.RoomId;
            device.MacAddress = request.MacAddress;
            device.Name = request.Name ?? null;
            device.Brand = request.Brand ?? null;
            device.Processor = request.Processor ?? null;
            device.RAM = request.RAM ?? null;
            device.Storage = request.Storage ?? null;
            device.IpAddress = request.IpAddress ?? null;
            device.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(device);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    [AllowAnonymous]
    [HttpPatch("mac/{macAddress}/offline")]
    public async Task<ActionResult<Device>> SetDeviceOffline(String macAddress)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstOrDefaultAsync();

            device.IpAddress = null;
            device.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(device);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
    }
    
    [AllowAnonymous]
    [HttpPut("mac/{macAddress}")]
    public async Task<ActionResult<Device>> UpdateDevice(string macAddress, [FromBody] Device request)
    {
        try
        {
            var device = await _context.Devices.Where(d => d.MacAddress == macAddress).FirstOrDefaultAsync();
            device.MacAddress = request.MacAddress;
            device.Brand = request.Brand ?? null;
            device.Processor = request.Processor ?? null;
            device.RAM = request.RAM ?? null;
            device.Storage = request.Storage ?? null;
            device.IpAddress = request.IpAddress ?? null;
            device.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(device);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Device>> SofDeleteDevice(int id)
    {
        try
        {
            var device = await _context.Devices.FindAsync(id);
            
            device.UpdatedAt = DateTime.Now;
            device.DeletedAt = DateTime.Now;
            
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