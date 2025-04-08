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

    /// <summary>
    /// Récupère la liste de tous les parcs (non supprimés).
    /// </summary>
    /// <returns>Liste des parcs</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<Park>>> GetAll()
    {
        var parks = await _context.Parks.Where(p => p.DeletedAt == null).ToListAsync();
        return Ok(parks);
    }

    /// <summary>
    /// Récupère un parc à partir de son ID.
    /// </summary>
    /// <param name="id">ID du parc</param>
    /// <returns>Le parc correspondant</returns>
    /// <response code="200">Parc trouvé</response>
    /// <response code="404">Aucun parc trouvé avec cet ID</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Park>> GetById(int id)
    {
        var park = await _context.Parks.FindAsync(id);
        if (park == null) return NotFound("Park not found !");
        
        return Ok(park);
    }

    /// <summary>
    /// Crée un nouveau parc.
    /// </summary>
    /// <param name="request">Données du parc à créer</param>
    /// <returns>Le parc nouvellement créé</returns>
    /// <response code="201">Parc créé avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
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

    /// <summary>
    /// Met à jour un parc existant à partir de son ID.
    /// </summary>
    /// <param name="id">ID du parc</param>
    /// <param name="request">Données mises à jour</param>
    /// <returns>Le parc mis à jour</returns>
    /// <response code="200">Mise à jour réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
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

    /// <summary>
    /// Supprime logiquement un parc (soft delete) via son ID.
    /// </summary>
    /// <param name="id">ID du parc</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
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