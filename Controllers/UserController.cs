using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using ParkManagerAPI.Services;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly ParkManagerContext _context;
    private readonly CustomLogger _logger;

    public UserController(ParkManagerContext context, CustomLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Récupère la liste de tous les utilisateurs.
    /// </summary>
    /// <returns>Liste des utilisateurs</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    /// <summary>
    /// Récupère un utilisateur à partir de son ID.
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <returns>L'utilisateur correspondant</returns>
    /// <response code="200">Utilisateur trouvé</response>
    /// <response code="404">Aucun utilisateur trouvé avec cet ID</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound("User not found !");
        
        return Ok(user);
    }
    
    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    /// <param name="request">Données de l'utilisateur à créer</param>
    /// <returns>Le nouvel utilisateur créé</returns>
    /// <response code="201">Utilisateur créé avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody]User request)
    {
        try
        {
            _context.Users.Add(request);
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "User",
                "UserController.CreateUser",
                $"Nouvel utilisateur créé : {request.Email}"
            );
            
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "User",
                "UserController.CreateUser",
                $"Erreur création utilisateur : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
    }

    /// <summary>
    /// Met à jour un utilisateur existant via son ID.
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <param name="request">Données mises à jour</param>
    /// <returns>L'utilisateur mis à jour</returns>
    /// <response code="200">Mise à jour réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
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
            
            await _logger.LogAsync(
                "info",
                "User",
                "UserController.UpdateUser",
                $"Utilisateur ID {user.Id} mis à jour"
            );
            
            return Ok(user);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "User",
                "UserController.UpdateUser",
                $"Erreur mise à jour utilisateur ID {id} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
        
    }
    
    /// <summary>
    /// Définit un utilisateur comme administrateur.
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <returns>L'utilisateur mis à jour avec le rôle administrateur</returns>
    /// <response code="200">Utilisateur mis à jour avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpPatch("{id}/admin")]
    public async Task<ActionResult<User>> SetUserAsAdmin(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            user.IsAdmin = true;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "User",
                "UserController.SetUserAsAdmin",
                $"Utilisateur ID {user.Id} défini comme administrateur"
            );
            
            return Ok(user);
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "User",
                "UserController.SetUserAsAdmin",
                $"Erreur lors de la promotion de l'utilisateur ID {id} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
        
    }
    
    /// <summary>
    /// Supprime logiquement un utilisateur (soft delete) via son ID.
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> SofDeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            
            user.UpdatedAt = DateTime.Now;
            user.DeletedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "User",
                "UserController.SofDeleteUser",
                $"Utilisateur ID {user.Id} supprimé logiquement"
            );
            
            return NoContent();
        }
        catch (Exception e)
        {
            await _logger.LogAsync(
                "error",
                "User",
                "UserController.SofDeleteUser",
                $"Erreur suppression logique utilisateur ID {id} : {e.Message}"
            );
            return StatusCode(500, "500 - Internal server error");
        }
    }
}