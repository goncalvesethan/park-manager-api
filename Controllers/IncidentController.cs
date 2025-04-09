using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkManagerAPI.Models;
using ParkManagerAPI.Services;
using Action = ParkManagerAPI.Models.Action;

namespace ParkManagerAPI.Controllers;

[ApiController]
[Route("api/incidents")]
[Authorize]

public class IncidentController : ControllerBase
{
    private readonly ParkManagerContext _context;
    private readonly CustomLogger _logger;

    public IncidentController(ParkManagerContext context, CustomLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Récupère la liste de tous les incidents (non supprimés).
    /// </summary>
    /// <returns>Liste des incidents</returns>
    /// <response code="200">Liste récupérée avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpGet]
    public async Task<ActionResult<List<Incident>>> GetAll()
    {
        var incidents = await _context.Incidents.Where(i => i.DeletedAt == null).ToListAsync();
        return Ok(incidents);
    }

    /// <summary>
    /// Récupère un incident à partir de son ID.
    /// </summary>
    /// <param name="id">ID de l'incident</param>
    /// <returns>L'incident correspondant</returns>
    /// <response code="200">Incident trouvé</response>
    /// <response code="404">Aucun incident trouvé avec cet ID</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Incident>> GetById(int id)
    {
        var incident = await _context.Incidents.FindAsync(id);
        if (incident == null) return NotFound("Incident not found !");
        
        return Ok(incident);
    }

    /// <summary>
    /// Crée un nouvel incident.
    /// </summary>
    /// <param name="request">Données de l'incident à créer</param>
    /// <returns>Le nouvel incident créé</returns>
    /// <response code="201">Incident créé avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
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
            
            await _logger.LogAsync(
                "info",
                "Incident",
                "IncidentController.CreateAction",
                $"Incident créé pour le poste ID {request.DeviceId} par l’utilisateur ID {request.ReporterId}"
            );
            
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Incident", "IncidentController.CreateAction", $"Erreur de création de l'incident : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
    }

    /// <summary>
    /// Marque un incident comme "fermé".
    /// </summary>
    /// <param name="id">ID de l'incident</param>
    /// <returns>L'incident mis à jour</returns>
    /// <response code="200">Incident fermé avec succès</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpPatch("{id}/close")]
    public async Task<ActionResult<Incident>> SetAsClosed(int id)
    {
        try
        {
            var incident = await _context.Incidents.FindAsync(id);

            incident.Status = "closed";
            incident.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Incident",
                "IncidentController.SetAsClosed",
                $"Incident ID {incident.Id} marqué comme fermé"
            );
            
            return Ok(incident);
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Incident", "IncidentController.SetAsClosed", $"Erreur de fermeture l'incident ID {id} : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
    }
    
    /// <summary>
    /// Met à jour un incident existant via son ID.
    /// </summary>
    /// <param name="id">ID de l'incident</param>
    /// <param name="request">Données mises à jour</param>
    /// <returns>L'incident mis à jour</returns>
    /// <response code="200">Mise à jour réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
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
            
            await _logger.LogAsync(
                "info",
                "Incident",
                "IncidentController.UpdateDevice",
                $"Incident ID {incident.Id} mis à jour"
            );
            
            return Ok(incident);
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Incident", "IncidentController.UpdateDevice", $"Erreur de la MAJ de l'incident ID {id} : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
        
    }

    /// <summary>
    /// Supprime logiquement un incident (soft delete).
    /// </summary>
    /// <param name="id">ID de l'incident</param>
    /// <returns>Réponse vide</returns>
    /// <response code="204">Suppression réussie</response>
    /// <response code="500">Erreur interne du serveur</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Action>> SofDeletePark(int id)
    {
        try
        {
            var incident = await _context.Incidents.FindAsync(id);
            
            incident.UpdatedAt = DateTime.Now;
            incident.DeletedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            
            await _logger.LogAsync(
                "info",
                "Incident",
                "IncidentController.SofDeletePark",
                $"Incident ID {incident.Id} supprimé logiquement (soft delete)"
            );
            
            return NoContent();
        }
        catch (Exception e)
        {
            await _logger.LogAsync("error", "Incident", "IncidentController.SofDeletePark", $"Erreur de la suppression logique de l'incident ID {id} : {e.Message}");
            return StatusCode(500, "500 - Internal server error");
        }
    }
}