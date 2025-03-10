using Microsoft.AspNetCore.Mvc;
using ParkManagerAPI.Models;
using ParkManagerAPI.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly ParkManagerContext _context;

    public AuthController(JwtService jwtService, ParkManagerContext dbContext)
    {
        _jwtService = jwtService;
        _context = dbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var token = _jwtService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
}

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}