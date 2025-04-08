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

    /// <summary>
    /// Récupère la liste de toutes les salles (non supprimées).
    /// </summary>
    /// <returns>Liste des salles</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetAll()
    {
        var rooms = await _context.Rooms.Where(r => r.DeletedAt == null).ToListAsync();
        return Ok(rooms);
    }

    /// <summary>
    /// Récupère une salle à partir de son ID.
    /// </summary>
    /// <param name="id">ID de la salle</param>
    /// <returns>La salle correspondante</returns>
    /// <response code="200">Salle trouvée</response>
    /// <response code="404">Aucune salle trouvée avec cet ID</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetById(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound("Room not found !");
        
        return Ok(room);
    }

    /// <summary>
    /// Crée une nouvelle salle.
    /// </summary>
    /// <param name="request">Données de la salle à créer</param>
    /// <returns>La nouvelle salle créée</returns>
    /// <response code="201">Salle créée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
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

    /// <summary>
    /// Met à jour une salle existante via son ID.
    /// </summary>
    /// <param name="id">ID de la salle</param>
    /// <param name="request">Données mises à jour</param>
    /// <returns>La salle mise à jour</returns>
    /// <response code="200">Mise à jour réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
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

    /// <summary>
    /// Supprime logiquement une salle (soft delete) via son ID.
    /// </summary>
    /// <param name="id">ID de la salle</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
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