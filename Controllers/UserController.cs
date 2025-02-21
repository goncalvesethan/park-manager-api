using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/users")]

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
}