using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly ParkManagerContext _context;

    public UserController(ParkManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound("User not found !");
        
        return Ok(user);
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody]User request)
    {
        try
        {
            _context.Users.Add(request);
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
    public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User request)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            user.Lastname = request.Lastname;
            user.Firstname = request.Firstname;
            user.Email = request.Email;
            user.Password = request.Password;
            user.IsAdmin = request.IsAdmin;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }
    
    [HttpPatch("{id}/admin")]
    public async Task<ActionResult<User>> SetUserAsAdmin(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            user.IsAdmin = true;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "500 - Internal server error");
        }
        
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> SofDeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            
            user.UpdatedAt = DateTime.Now;
            user.DeletedAt = DateTime.Now;
            
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