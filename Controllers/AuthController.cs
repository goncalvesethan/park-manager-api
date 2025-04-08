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
    
    /// <summary>
    /// Authentifie un utilisateur administrateur et génère un token JWT.
    /// </summary>
    /// <param name="request">Données de connexion de l'utilisateur (email et mot de passe)</param>
    /// <returns>Un token JWT si l'authentification réussit</returns>
    /// <response code="200">Connexion réussie, retourne le token</response>
    /// <response code="401">Identifiants invalides</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.Where(u => u.IsAdmin == true).FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized();
        }

        var token = _jwtService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
}

/// <summary>
/// Représente la requête d'authentification avec email et mot de passe.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Adresse email de l'utilisateur.
    /// </summary>
    public required string Email { get; set; }
    /// <summary>
    /// Mot de passe de l'utilisateur.
    /// </summary>
    public required string Password { get; set; }
}